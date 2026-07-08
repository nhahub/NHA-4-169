using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.UpdateProviderBio
{
	public sealed class UpdateProviderBioCommandHandler
		: ICommandHandler<UpdateProviderBioCommand, UpdateProviderBioResponse>
	{
		private readonly IRepository<ProviderProfile, string> _providerProfileRepository;
		private readonly IUnitOfWork _unitOfWork;

		public UpdateProviderBioCommandHandler(
			IRepository<ProviderProfile, string> providerProfileRepository,
			IUnitOfWork unitOfWork)
		{
			_providerProfileRepository = providerProfileRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<UpdateProviderBioResponse>> Handle(
			UpdateProviderBioCommand request, CancellationToken ct)
		{
			var profile = await _providerProfileRepository.GetByIdAsync(request.ProviderProfileId, ct);

			if (profile is null)
				return Result<UpdateProviderBioResponse>.Failure("Provider profile not found.");

			profile.UpdateBio(request.Bio, request.UpdatedBy);

			_providerProfileRepository.Update(profile);
			await _unitOfWork.SaveChangesAsync(ct);

			var response = new UpdateProviderBioResponse(profile.Id, profile.Bio!);
			return Result<UpdateProviderBioResponse>.Success(response);
		}
	}
}
