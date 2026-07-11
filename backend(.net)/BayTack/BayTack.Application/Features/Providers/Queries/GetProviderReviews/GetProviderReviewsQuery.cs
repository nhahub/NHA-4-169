using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Queries.GetProviderReviews
{
	public sealed record GetProviderReviewsQuery(
		string ProviderId, string? Filter, int Page, int Limit) : IQuery<ProviderReviewsListResponse>;

	public sealed record ReviewResponse(
		string ReviewId, string CustomerId, string AuthorName, int Rating, string? Text,
		string? ServiceName, string OrderId, DateTime CreatedAt, string? ProviderResponse);

	public sealed record ProviderReviewsListResponse(List<ReviewResponse> Reviews, int Total);
}
