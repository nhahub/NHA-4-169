using BayTack.Application.Abstractions.Messaging;
using BayTack.Domain.Enums;

namespace BayTack.Application.Features.Orders.Commands.UpdateBookingStatus
{
	public sealed record UpdateBookingStatusCommand(
		string OrderId, OrderStatus NewStatus, string ChangedBy) : ICommand<UpdateBookingStatusResponse>;

	public sealed record UpdateBookingStatusResponse(string OrderId, string Status);
}
