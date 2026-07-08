using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;
using BayTack.Domain.ValueObjects;

namespace BayTack.Application.Features.Providers.Commands.SetWorkshopAddress
{
	public sealed class SetWorkshopAddressCommandHandler
		: ICommandHandler<SetWorkshopAddressCommand, SetWorkshopAddressResponse>
	{
		private readonly IRepository<ProviderProfile, string> _providerProfileRepository;
		private readonly IUnitOfWork _unitOfWork;

		public SetWorkshopAddressCommandHandler(
			IRepository<ProviderProfile, string> providerProfileRepository,
			IUnitOfWork unitOfWork)
		{
			_providerProfileRepository = providerProfileRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<SetWorkshopAddressResponse>> Handle(
			SetWorkshopAddressCommand request, CancellationToken ct)
		{
			var profile = await _providerProfileRepository.GetByIdAsync(request.ProviderProfileId, ct);

			if (profile is null)
				return Result<SetWorkshopAddressResponse>.Failure("Provider profile not found.");

			var address = Address.Create(request.Details, request.CityId, request.AreaId);
			profile.SetWorkshopAddress(address, request.UpdatedBy);

			_providerProfileRepository.Update(profile);
			await _unitOfWork.SaveChangesAsync(ct);

			var response = new SetWorkshopAddressResponse(
				profile.Id,
				address.Details,
				address.CityId,
				address.AreaId);

			return Result<SetWorkshopAddressResponse>.Success(response);
		}
	}
}
