using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Providers.Queries.GetAllPortfolioItems;

namespace BayTack.Application.Features.Providers.Queries.GetPortfolioItemById
{
	public sealed record GetPortfolioItemByIdQuery(string ItemId) : IQuery<PortfolioItemResponse>;
}
