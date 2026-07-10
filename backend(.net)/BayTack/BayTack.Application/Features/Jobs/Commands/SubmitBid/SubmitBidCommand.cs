using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Jobs.Commands.SubmitBid
{
	public sealed record SubmitBidCommand(
		string JobId,
		string ProviderId,
		decimal Price,
		string Currency,
		int DurationInDays,
		string? Notes) : ICommand<SubmitBidResponse>;

	public sealed record SubmitBidResponse(
		string BidId,
		string JobId,
		string ProviderId,
		decimal ProposedPrice,
		string Currency,
		int DurationInDays,
		string Status);
}
