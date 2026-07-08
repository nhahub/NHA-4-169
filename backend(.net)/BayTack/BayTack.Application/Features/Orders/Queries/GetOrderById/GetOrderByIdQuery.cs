using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Orders.Common;

namespace BayTack.Application.Features.Orders.Queries.GetOrderById
{
	public sealed record GetOrderByIdQuery(string CustomerId, string OrderId) : IQuery<OrderDetailResponse>;
}
