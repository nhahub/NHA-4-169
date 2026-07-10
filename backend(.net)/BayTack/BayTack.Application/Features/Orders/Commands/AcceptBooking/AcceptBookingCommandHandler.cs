using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.OrderAggregate;
using BayTack.Domain.Enums;

namespace BayTack.Application.Features.Orders.Commands.AcceptBooking
{
	public sealed class AcceptBookingCommandHandler : ICommandHandler<AcceptBookingCommand, AcceptBookingResponse>
	{
		private readonly IRepository<Order, string> _orderRepository;
		private readonly IUnitOfWork _unitOfWork;

		public AcceptBookingCommandHandler(IRepository<Order, string> orderRepository, IUnitOfWork unitOfWork)
		{
			_orderRepository = orderRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<AcceptBookingResponse>> Handle(AcceptBookingCommand request, CancellationToken ct)
		{
			var order = await _orderRepository.GetByIdAsync(request.OrderId, ct);

			if (order is null)
				return Result<AcceptBookingResponse>.Failure("Booking not found.");

			try
			{
				order.ChangeStatus(OrderStatus.Confirmed, request.ChangedBy);
			}
			catch (InvalidOperationException ex)
			{
				return Result<AcceptBookingResponse>.Failure(ex.Message);
			}

			_orderRepository.Update(order);
			await _unitOfWork.SaveChangesAsync(ct);

			return Result<AcceptBookingResponse>.Success(new AcceptBookingResponse(order.Id, order.Status.ToString()));
		}
	}
}
