using BayTack.Application.Features.Orders.Common;

namespace BayTack.Application.Abstractions.IRepository
{
    /// <summary>
    /// Read-side repository for the customer Orders list/detail screens. Kept separate from
    /// the generic IRepository&lt;Order,string&gt; because these queries join across the Order
    /// and CustomerJob aggregates plus AppUser (Identity) - the same reason IUserRepository /
    /// IConversationRepository exist instead of forcing everything through Specification&lt;T&gt;.
    /// </summary>
    public interface IOrdersReadRepository
    {
        /// <summary>All orders belonging to a customer's jobs. statusGroup is "active" | "completed" | "cancelled" | null (= all).</summary>
        Task<List<OrderResponse>> GetForCustomerAsync(string customerId, string? statusGroup, CancellationToken ct = default);

        /// <summary>A single order's full detail, scoped to its owning customer (never returns another customer's order).</summary>
        Task<OrderDetailResponse?> GetByIdForCustomerAsync(string customerId, string orderId, CancellationToken ct = default);

        /// <summary>Small lookup used to build an OrderResponse right after a command mutates an Order in-memory
        /// (a fresh read-side query would miss the not-yet-saved change, so command handlers assemble the
        /// response themselves and only need the provider's display name from here).</summary>
        Task<string?> GetProviderNameAsync(string providerId, CancellationToken ct = default);
    }
}