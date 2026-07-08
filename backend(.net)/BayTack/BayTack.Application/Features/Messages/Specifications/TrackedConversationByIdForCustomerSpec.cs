using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.Messaging;

namespace BayTack.Application.Features.Messages.Specifications
{
	/// <summary>Tracked (feeds a command): a single conversation by id, scoped to its owning
	/// customer, with messages included so Conversation.AddMessage(...) has the current
	/// state available.</summary>
	public sealed class TrackedConversationByIdForCustomerSpec : Specification<Conversation>
	{
		public TrackedConversationByIdForCustomerSpec(string conversationId, string customerId)
			: base(c => c.Id == conversationId && c.CustomerId == customerId)
		{
			AddInclude(c => c.Messages);
			EnableTracking();
		}
	}
}
