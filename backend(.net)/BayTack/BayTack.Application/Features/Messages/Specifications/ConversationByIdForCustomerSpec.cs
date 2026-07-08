using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.Messaging;

namespace BayTack.Application.Features.Messages.Specifications
{
	/// <summary>Read-only: a single conversation by id, scoped to its owning customer so one
	/// customer can never read another customer's thread by guessing an id.</summary>
	public sealed class ConversationByIdForCustomerSpec : Specification<Conversation>
	{
		public ConversationByIdForCustomerSpec(string conversationId, string customerId)
			: base(c => c.Id == conversationId && c.CustomerId == customerId)
		{
			AddInclude(c => c.Messages);
		}
	}
}
