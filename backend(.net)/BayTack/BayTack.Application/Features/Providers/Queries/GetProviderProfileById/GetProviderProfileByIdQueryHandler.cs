using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Queries.GetProviderProfileById;

public sealed class GetProviderProfileByIdQueryHandler(
    IRepository<ProviderProfile, string> providerProfiles) : IQueryHandler<GetProviderProfileByIdQuery, ProviderProfileResponse>
{
    public async Task<Result<ProviderProfileResponse>> Handle(GetProviderProfileByIdQuery query, CancellationToken cancellationToken)
    {
        var providerProfile = await providerProfiles.GetByIdAsync(query.ProviderProfileId, cancellationToken);
        if (providerProfile is null)
        {
            return Result<ProviderProfileResponse>.Failure("Provider profile not found.");
        }

        return Result<ProviderProfileResponse>.Success(new ProviderProfileResponse(
            providerProfile.Id,
            providerProfile.UserId,
            providerProfile.ProviderType.ToString(),
            providerProfile.VerificationStatus.ToString(),
            providerProfile.YearsOfExperience,
            providerProfile.Bio,
            providerProfile.WorkshopAddress?.Details,
            providerProfile.WorkshopAddress?.CityId,
            providerProfile.WorkshopAddress?.AreaId,
            providerProfile.Documents.Count,
            providerProfile.Portfolio.Count,
            providerProfile.Availabilities.Count));
    }
}
