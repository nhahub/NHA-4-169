using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Queries.ExportProviderReviewsCsv
{
	public sealed record ExportProviderReviewsCsvQuery(string ProviderId, string? Filter) : IQuery<string>;
}
