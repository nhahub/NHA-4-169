using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Jobs.Commands.RetractBid
{
	public sealed record RetractBidCommand(string BidId) : ICommand<RetractBidResponse>;

	public sealed record RetractBidResponse(string BidId, string Status);
}
