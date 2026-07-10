using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Orders.Commands.AcceptBooking
{
	public sealed record AcceptBookingCommand(string OrderId, string ChangedBy) : ICommand<AcceptBookingResponse>;

	public sealed record AcceptBookingResponse(string OrderId, string Status);
}
