using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Orders.Common;

namespace BayTack.Application.Features.Orders.Commands.CreateOrder
{
	/// <summary>Books a catalog service directly (no bid/negotiation step). Tier is "basic" | "standard" | "premium".</summary>
	public sealed record CreateOrderCommand(string CustomerId, string ServiceId, string? Tier) : ICommand<OrderResponse>;
}
