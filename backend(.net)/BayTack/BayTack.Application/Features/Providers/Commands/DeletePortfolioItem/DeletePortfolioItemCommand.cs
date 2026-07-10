using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Commands.DeletePortfolioItem
{
	public sealed record DeletePortfolioItemCommand(string ItemId) : ICommand<DeletePortfolioItemResponse>;

	public sealed record DeletePortfolioItemResponse(string ItemId, bool Deleted);
}
