using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Commands.UpdatePortfolioItem
{
	public sealed record UpdatePortfolioItemCommand(
		string ItemId,
		string Title,
		string? Description,
		string? ImageUrl) : ICommand<UpdatePortfolioItemResponse>;

	public sealed record UpdatePortfolioItemResponse(
		string ItemId,
		string Title,
		string? Description,
		string? ImageUrl);
}
