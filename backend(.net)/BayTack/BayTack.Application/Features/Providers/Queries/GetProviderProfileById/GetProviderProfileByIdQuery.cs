using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Queries.GetProviderProfileById
{
	public sealed record GetProviderProfileByIdQuery(string ProviderProfileId) : IQuery<ProviderProfileResponse>;

	public sealed record ProviderProfileResponse(
		string ProviderProfileId,
		string UserId,
		string ProviderType,
		string VerificationStatus,
		int YearsOfExperience,
		string? Bio,
		string? WorkshopAddressDetails,
		int? WorkshopAddressCityId,
		int? WorkshopAddressAreaId,
		int DocumentsCount,
		int PortfolioCount,
		int AvailabilitiesCount);
}
