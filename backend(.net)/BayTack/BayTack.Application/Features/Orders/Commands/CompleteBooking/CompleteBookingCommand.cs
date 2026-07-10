using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Orders.Commands.CompleteBooking
{
	public sealed record CompleteBookingCommand(string OrderId, string ChangedBy) : ICommand<CompleteBookingResponse>;

	public sealed record CompleteBookingResponse(string OrderId, string Status, DateTime? EndDate);
}
