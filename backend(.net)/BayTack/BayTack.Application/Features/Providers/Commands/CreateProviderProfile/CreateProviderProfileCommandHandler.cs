using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.CreateProviderProfile
{
	public sealed class CreateProviderProfileCommandHandler
		: ICommandHandler<CreateProviderProfileCommand, CreateProviderProfileResponse>
	{
		private readonly IRepository<ProviderProfile, string> _providerProfileRepository;
		private readonly IUnitOfWork _unitOfWork;

		public CreateProviderProfileCommandHandler(
			IRepository<ProviderProfile, string> providerProfileRepository,
			IUnitOfWork unitOfWork)
		{
			_providerProfileRepository = providerProfileRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<CreateProviderProfileResponse>> Handle(
			CreateProviderProfileCommand request, CancellationToken ct)
		{
			var alreadyExists = await _providerProfileRepository.AnyAsync(
				new ProviderProfileByUserIdSpec(request.UserId), ct);

			if (alreadyExists)
				return Result<CreateProviderProfileResponse>.Failure(
					"A provider profile already exists for this user.");

			var profile = ProviderProfile.Create(
				request.UserId,
				request.ProviderType,
				request.YearsOfExperience,
				request.Bio);

			_providerProfileRepository.Add(profile);

			await _unitOfWork.SaveChangesAsync(ct);

			var response = new CreateProviderProfileResponse(
				profile.Id,
				profile.UserId,
				profile.ProviderType.ToString(),
				profile.VerificationStatus.ToString(),
				profile.YearsOfExperience,
				profile.Bio);

			return Result<CreateProviderProfileResponse>.Success(response);
		}
	}
}
