using BayTack.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Jobs.Commands.PlaceBid
{

	public sealed record PlaceBidCommand(
		int JobId,
		int ProviderId,
		decimal ProposedPrice,
		string Currency,
		int DurationInDays,
		string? Notes) : ICommand<PlaceBidResponse>;

	public sealed record PlaceBidResponse(int BidId, string Status);

}
