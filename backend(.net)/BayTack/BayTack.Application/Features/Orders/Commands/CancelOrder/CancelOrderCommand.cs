using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Orders.Common;

namespace BayTack.Application.Features.Orders.Commands.CancelOrder
{
	public sealed record CancelOrderCommand(string CustomerId, string OrderId) : ICommand<OrderResponse>;
}
