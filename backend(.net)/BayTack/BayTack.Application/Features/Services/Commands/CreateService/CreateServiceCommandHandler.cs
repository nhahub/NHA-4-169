using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ServiceAggregate;
using BayTack.Domain.ValueObjects;


namespace BayTack.Application.Features.Services.Commands.CreateService
{
	public sealed class CreateServiceCommandHandler : ICommandHandler<CreateServiceCommand, string>
	{
		private readonly IRepository<Service, string> _serviceRepository;
		private readonly IRepository<ServiceCategory, string> _categoryRepository;
		public CreateServiceCommandHandler(
			IRepository<Service, string> serviceRepository,
			IRepository<ServiceCategory, string> categoryRepository )
		{
			_serviceRepository = serviceRepository;
			_categoryRepository = categoryRepository;
		}
		public async Task<Result<string>> Handle(CreateServiceCommand request, CancellationToken ct)
		{
			var category = await _categoryRepository.GetByIdAsync(request.CategoryId, ct);
			if (category is null)
				return Result<string>.Failure("Selected category does not exist.");
			var service = Service.Create(
				request.CategoryId,
				request.Name,
				Money.Create(request.MinPrice, string.IsNullOrWhiteSpace(request.Currency) ? "EGP" : request.Currency),
				Money.Create(request.MaxPrice, string.IsNullOrWhiteSpace(request.Currency) ? "EGP" : request.Currency),
				request.AllowCredit,
				request.AllowInstallments,
				request.Description);

			if (request.PaymentMethodIds != null)
			{
				foreach (var pmId in request.PaymentMethodIds)
				{
					//service.AllowPaymentMethod(pmId);
				}
			}
			_serviceRepository.Add(service);

			return Result<string>.Success(service.Id);
		}
	}
}
