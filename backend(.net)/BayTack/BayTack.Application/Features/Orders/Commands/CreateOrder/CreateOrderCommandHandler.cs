using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Orders.Common;
using BayTack.Domain.Entities.JobAggregate;
using BayTack.Domain.Entities.ServiceAggregate;
using BayTack.Domain.ValueObjects;

namespace BayTack.Application.Features.Orders.Commands.CreateOrder
{
	/// <summary>
	/// NOTE: an Order can only be created with a ProviderId already assigned (see
	/// Order.Create / AcceptBidCommandHandler + BidAcceptedEventHandler for that flow), and
	/// direct catalog booking has no provider-selection step in the domain yet. Rather than
	/// fabricating a provider assignment, this creates the underlying CustomerJob (the real
	/// "booking request") and returns it in the Order shape the frontend expects, with
	/// status "Pending" until a provider is assigned - the same way a job normally starts
	/// its life before a bid is accepted. Flagging this gap rather than guessing a matching
	/// algorithm.
	/// </summary>
	public sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, OrderResponse>
	{
		private readonly IRepository<Service, string> _serviceRepository;
		private readonly IRepository<CustomerJob, string> _jobRepository;
		private readonly IUserRepository _userRepository;

		public CreateOrderCommandHandler(
			IRepository<Service, string> serviceRepository,
			IRepository<CustomerJob, string> jobRepository,
			IUserRepository userRepository)
		{
			_serviceRepository = serviceRepository;
			_jobRepository = jobRepository;
			_userRepository = userRepository;
		}

		public async Task<Result<OrderResponse>> Handle(CreateOrderCommand request, CancellationToken ct)
		{
			var service = await _serviceRepository.GetByIdAsync(request.ServiceId, ct);
			if (service is null)
				return Result<OrderResponse>.Failure("Service not found.");

			var address = await _userRepository.GetAddressAsync(request.CustomerId, ct);
			if (address is null)
				return Result<OrderResponse>.Failure("Please add an address to your profile before booking a service.");

			var tier = request.Tier?.ToLowerInvariant() ?? "basic";
			var price = tier switch
			{
				"premium" => service.MaxPrice,
				"standard" => Money.Create((service.MinPrice.Amount + service.MaxPrice.Amount) / 2, service.MinPrice.Currency),
				_ => service.MinPrice
			};

			var job = CustomerJob.Create(
				request.CustomerId,
				request.ServiceId,
				title: $"{service.Name} booking",
				description: $"Direct booking of \"{service.Name}\" ({tier} tier).",
				location: address);

			_jobRepository.Add(job);
			// NOTE: no SaveChangesAsync call here - UnitOfWorkBehavior does it automatically.

			const string status = "Pending";
			return new OrderResponse(
				job.Id,
				job.ServiceId,
				job.Title,
				"Pending assignment",
				null,
				price.Amount,
				status,
				OrderResponse.ProgressFor(status));
		}
	}
}
