using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Orders.Queries.GetAllBookings;

namespace BayTack.Application.Features.Orders.Queries.GetBookingById
{
	public sealed record GetBookingByIdQuery(string OrderId) : IQuery<BookingResponse>;
}
