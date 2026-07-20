using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Queries.GetMyProviderProfile
{
	/// <summary>Fetches the provider profile belonging to the currently signed-in user.</summary>
	public sealed record GetMyProviderProfileQuery(string UserId) : IQuery<MyProviderProfileResponse>;

	/// <summary>
	/// Drives the provider dashboard header + "under review" banner.
	/// HasSubmittedDocuments distinguishes "hasn't uploaded verification papers yet" from
	/// "papers uploaded, waiting on admin review" - both are VerificationStatus == Pending,
	/// but the frontend copy/CTA should differ between them.
	/// </summary>
	public sealed record MyProviderProfileResponse(
		string ProviderProfileId,
		string Name,
		string? Phone,
		string ProviderType,
		string VerificationStatus,
		string? CategoryId,
		string? CategoryName,
		bool HasSubmittedDocuments,
		string? SuspendReason);
}
