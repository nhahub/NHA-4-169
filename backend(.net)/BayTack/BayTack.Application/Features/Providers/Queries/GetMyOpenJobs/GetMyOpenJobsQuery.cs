using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Queries.GetMyOpenJobs
{
	/// <summary>Open customer job requests matching the signed-in provider's own service category.</summary>
	public sealed record GetMyOpenJobsQuery(string UserId) : IQuery<IReadOnlyList<ProviderJobFeedItemResponse>>;

	public sealed record ProviderJobFeedItemResponse(
		string Id,
		string Title,
		string Description,
		string CategoryId,
		string CategoryName,
		string Location,
		DateTime CreatedAt);
}
