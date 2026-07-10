using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Orders.Commands.DeclineBooking
{
	public sealed record DeclineBookingCommand(string OrderId, string ChangedBy) : ICommand<DeclineBookingResponse>;

	public sealed record DeclineBookingResponse(string OrderId, string Status);
}
