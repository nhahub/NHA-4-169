using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Jobs.Commands.PlaceBid
{
    public sealed record PlaceBidCommand(
        string JobId,
        string ProviderId,
        decimal ProposedPrice,
        string Currency,
        int DurationInDays,
        string? Notes) : ICommand<PlaceBidResponse>;

    public sealed record PlaceBidResponse(string BidId, string Status);
}