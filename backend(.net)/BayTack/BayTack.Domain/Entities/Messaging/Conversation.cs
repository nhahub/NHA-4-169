using BayTack.Domain.Common.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BayTack.Domain.Entities.Messaging
{
    /// <summary>Aggregate root for a single customer/provider chat thread. One conversation
    /// per (CustomerId, ProviderId) pair - enforced by a unique index in the EF configuration,
    /// not re-checked here since Create() has no repository access to look for duplicates.</summary>
    public class Conversation : BaseEntity<string>
    {
        public string CustomerId { get; private set; } = string.Empty;
        public string ProviderId { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime LastMessageAt { get; private set; } = DateTime.UtcNow;

        private readonly List<Message> _messages = new();
        public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

        private Conversation() { }

        public static Conversation Create(string customerId, string providerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("Customer id is required.", nameof(customerId));
            if (string.IsNullOrWhiteSpace(providerId))
                throw new ArgumentException("Provider id is required.", nameof(providerId));
            if (customerId == providerId)
                throw new ArgumentException("A conversation must be between two different users.");

            var now = DateTime.UtcNow;
            return new Conversation
            {
                // Same PK-generation gap as Message.Create - see note there.
                Id = Guid.NewGuid().ToString(),
                CustomerId = customerId,
                ProviderId = providerId,
                CreatedAt = now,
                LastMessageAt = now
            };
        }

        /// <summary>Appends a message from either side of the conversation and bumps
        /// LastMessageAt. senderId must be one of the two participants.</summary>
        public Message AddMessage(string senderId, string text)
        {
            if (senderId != CustomerId && senderId != ProviderId)
                throw new InvalidOperationException("Sender is not a participant in this conversation.");

            var message = Message.Create(Id, senderId, text);
            _messages.Add(message);
            LastMessageAt = message.SentAt;
            return message;
        }

        /// <summary>Marks every message not sent by readerId as read - used when a
        /// participant opens the thread.</summary>
        public void MarkReadBy(string readerId)
        {
            foreach (var message in _messages.Where(m => m.SenderId != readerId && !m.IsRead))
                message.MarkAsRead();
        }
    }
}