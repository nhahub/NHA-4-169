using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Commands.AddPortfolioItem
{
	public sealed record AddPortfolioItemCommand(
		string ProviderProfileId,
		string Title,
		string? Description,
		string? ImageUrl) : ICommand<AddPortfolioItemResponse>;

	public sealed record AddPortfolioItemResponse(
		string ProviderProfileId,
		string PortfolioItemId,
		string Title,
		string? Description,
		string? ImageUrl);
}
