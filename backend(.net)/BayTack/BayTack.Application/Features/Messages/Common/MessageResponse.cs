using BayTack.Domain.Entities.Messaging;

namespace BayTack.Application.Features.Messages.Common
{
	/// <summary>Shape expected by Front_end/customer/app/messages thread rendering
	/// (bt_c_messages[].thread[]).</summary>
	public sealed record MessageResponse(
		string Id,
		string SenderId,
		bool IsMine,
		string Text,
		bool Read,
		DateTime SentAt)
	{
		public static MessageResponse FromEntity(Message m, string currentUserId) => new(
			m.Id,
			m.SenderId,
			m.SenderId == currentUserId,
			m.Text,
			m.IsRead,
			m.SentAt);
	}
}
