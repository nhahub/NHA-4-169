namespace BayTack.Application.Features.Messages.Common
{
	/// <summary>Shape expected by GET /customer/messages (bt_c_messages list view):
	/// { id, providerId, providerName, avatar, lastMessage, unreadCount }.
	/// Built entirely inside IConversationRepository since it joins Conversation+Message
	/// (Messaging aggregate) with AppUser (Identity) - outside what a single Specification
	/// over one aggregate can express.</summary>
	public sealed record ConversationSummaryResponse(
		string Id,
		string ProviderId,
		string ProviderName,
		string? Avatar,
		string? LastMessage,
		DateTime UpdatedAt,
		int UnreadCount);
}
