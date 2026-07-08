using BayTack.Domain.Entities.UserFeatures;
using BayTack.Domain.Enums;

namespace BayTack.Application.Features.Notifications.Common
{
	/// <summary>Shape expected by Front_end/customer/app/notifications (bt_c_notifications).</summary>
	public sealed record NotificationResponse(
		string Id,
		string Group,
		bool Read,
		string Icon,
		string Title,
		string Body,
		DateTime CreatedAt)
	{
		// The Notification entity only stores Type - Group/Icon aren't persisted fields,
		// so they're derived here. If the frontend needs custom icons per notification
		// (not just per type) that has to become a real column instead.
		public static NotificationResponse FromEntity(Notification n) => new(
			n.Id,
			GroupFor(n.Type),
			n.IsRead,
			IconFor(n.Type),
			n.Title,
			n.Message,
			n.CreatedAt);

		private static string GroupFor(NotificationType type) => type switch
		{
			NotificationType.Order => "orders",
			NotificationType.Bid => "bids",
			NotificationType.Payment => "payments",
			NotificationType.Warning => "alerts",
			NotificationType.Success => "alerts",
			NotificationType.System => "system",
			_ => "general"
		};

		private static string IconFor(NotificationType type) => type switch
		{
			NotificationType.Order => "package",
			NotificationType.Bid => "gavel",
			NotificationType.Payment => "credit-card",
			NotificationType.Warning => "alert-triangle",
			NotificationType.Success => "check-circle",
			NotificationType.System => "settings",
			_ => "bell"
		};
	}
}
