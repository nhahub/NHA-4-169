using BayTack.Domain.Entities.Messaging;

namespace BayTack.Application.Features.Messages.Common
{
	/// <summary>Shape expected by GET /customer/messages/{id}: the conversation plus its
	/// full message thread.</summary>
	public sealed record ConversationDetailResponse(
		string Id,
		string ProviderId,
		string ProviderName,
		string? Avatar,
		List<MessageResponse> Thread)
	{
		public static ConversationDetailResponse FromEntity(
			Conversation conversation, string providerName, string? avatar, string currentUserId) => new(
			conversation.Id,
			conversation.ProviderId,
			providerName,
			avatar,
			conversation.Messages
				.OrderBy(m => m.SentAt)
				.Select(m => MessageResponse.FromEntity(m, currentUserId))
				.ToList());
	}
}
