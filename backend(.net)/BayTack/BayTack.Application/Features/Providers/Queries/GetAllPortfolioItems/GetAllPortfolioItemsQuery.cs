using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Queries.GetAllPortfolioItems
{
	public sealed record GetAllPortfolioItemsQuery(string ProviderProfileId) : IQuery<List<PortfolioItemResponse>>;

	public sealed record PortfolioItemResponse(
		string ItemId,
		string ProviderProfileId,
		string Title,
		string? Description,
		string? ImageUrl);
}
