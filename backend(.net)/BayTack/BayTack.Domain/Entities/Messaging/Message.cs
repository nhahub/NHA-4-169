using BayTack.Domain.Common.BaseEntity;
using System;

namespace BayTack.Domain.Entities.Messaging
{
    /// <summary>A single chat message inside a Conversation. Not an aggregate root on its
    /// own - always created and mutated through Conversation.AddMessage(...).</summary>
    public class Message : BaseEntity<string>
    {
        public string ConversationId { get; private set; } = string.Empty;
        public string SenderId { get; private set; } = string.Empty;
        public string Text { get; private set; } = string.Empty;
        public DateTime SentAt { get; private set; } = DateTime.UtcNow;
        public bool IsRead { get; private set; }

        private Message() { }

        internal static Message Create(string conversationId, string senderId, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Message text is required.", nameof(text));

            return new Message
            {
                // NOTE: string PKs aren't configured for value generation anywhere in this
                // project (Notification/Favorite/CustomerJob/etc. have the same gap - their
                // Create() factories never set Id). Assigning it explicitly here so this
                // entity doesn't inherit that bug; flagging it rather than silently patching
                // every other aggregate.
                Id = Guid.NewGuid().ToString(),
                ConversationId = conversationId,
                SenderId = senderId,
                Text = text,
                SentAt = DateTime.UtcNow
            };
        }

        public void MarkAsRead() => IsRead = true;
    }
}