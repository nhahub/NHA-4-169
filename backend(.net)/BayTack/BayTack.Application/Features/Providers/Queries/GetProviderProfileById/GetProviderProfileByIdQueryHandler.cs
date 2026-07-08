using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Queries.GetProviderProfileById
{
	public sealed class GetProviderProfileByIdQueryHandler
		: IQueryHandler<GetProviderProfileByIdQuery, ProviderProfileResponse>
	{
		private readonly IRepository<ProviderProfile, string> _providerProfileRepository;

		public GetProviderProfileByIdQueryHandler(IRepository<ProviderProfile, string> providerProfileRepository)
		{
			_providerProfileRepository = providerProfileRepository;
		}

		public async Task<Result<ProviderProfileResponse>> Handle(
			GetProviderProfileByIdQuery request, CancellationToken ct)
		{
			var profile = await _providerProfileRepository.GetByIdAsync(request.ProviderProfileId, ct);

			if (profile is null)
				return Result<ProviderProfileResponse>.Failure("Provider profile not found.");

			var response = new ProviderProfileResponse(
				profile.Id,
				profile.UserId,
				profile.ProviderType.ToString(),
				profile.VerificationStatus.ToString(),
				profile.YearsOfExperience,
				profile.Bio,
				profile.WorkshopAddress?.Details,
				profile.WorkshopAddress?.CityId,
				profile.WorkshopAddress?.AreaId,
				profile.Documents.Count,
				profile.Portfolio.Count,
				profile.Availabilities.Count);

			return Result<ProviderProfileResponse>.Success(response);
		}
	}
}
