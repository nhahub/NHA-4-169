using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Queries.GetProviderReviewStats
{
	public sealed record GetProviderReviewStatsQuery(string ProviderId) : IQuery<ProviderReviewStatsResponse>;

	public sealed record ProviderReviewStatsResponse(
		double AverageRating, int TotalReviews, double SatisfactionPct, int FiveStarCount, int WeeklyCount);
}
