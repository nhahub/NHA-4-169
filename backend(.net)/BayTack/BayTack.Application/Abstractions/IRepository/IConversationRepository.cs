using BayTack.Application.Features.Messages.Common;

namespace BayTack.Application.Abstractions.IRepository
{
    /// <summary>
    /// Generic IRepository&lt;Conversation, string&gt; + specs cover the tracked command path
    /// (see TrackedConversationByIdForCustomerSpec). This repository exists only for the read
    /// shapes that need a join across the Messaging aggregate into Identity (AppUser) for the
    /// provider's display name - the same reason IUserRepository exists next to the generic
    /// repository for Users.
    /// </summary>
    public interface IConversationRepository
    {
        Task<List<ConversationSummaryResponse>> GetConversationsForCustomerAsync(
            string customerId, CancellationToken ct = default);

        Task<string?> GetDisplayNameAsync(string userId, CancellationToken ct = default);
    }
}