using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Commands.AddPortfolioItem;

/// <summary>Attaches a new portfolio item to an existing provider profile.</summary>
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
