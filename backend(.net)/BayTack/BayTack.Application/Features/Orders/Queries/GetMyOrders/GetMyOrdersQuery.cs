using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Orders.Common;

namespace BayTack.Application.Features.Orders.Queries.GetMyOrders
{
	/// <summary>Status is the "active" | "completed" | "cancelled" bucket from the ?status= query string.</summary>
	public sealed record GetMyOrdersQuery(string CustomerId, string? Status) : IQuery<List<OrderResponse>>;
}
