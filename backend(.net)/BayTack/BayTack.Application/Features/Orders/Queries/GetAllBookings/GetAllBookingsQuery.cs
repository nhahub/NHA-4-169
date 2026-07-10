using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Orders.Queries.GetAllBookings
{
	public sealed record GetAllBookingsQuery(string ProviderId, string? Status) : IQuery<List<BookingResponse>>;

	public sealed record BookingResponse(
		string OrderId,
		string CustomerJobId,
		string ProviderId,
		decimal FinalPrice,
		string Currency,
		DateTime StartDate,
		DateTime? EndDate,
		string Status);
}
