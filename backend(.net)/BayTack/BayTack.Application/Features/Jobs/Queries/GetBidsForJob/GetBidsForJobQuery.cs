using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Jobs.Queries.GetBidsForJob
{
	public sealed record GetBidsForJobQuery(string JobId) : IQuery<List<BidResponse>>;

	public sealed record BidResponse(
		string BidId,
		string ProviderId,
		decimal ProposedPrice,
		string Currency,
		int DurationInDays,
		string? Notes,
		string Status);
}
