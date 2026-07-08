using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.UserFeatures;

namespace BayTack.Application.Features.Notifications.Specifications
{
	/// <summary>
	/// Tracked (feeds a command): a single notification, scoped to its owner so one customer
	/// can never mark another customer's notification as read by guessing an id.
	/// </summary>
	public sealed class TrackedNotificationByIdForUserSpec : Specification<Notification>
	{
		public TrackedNotificationByIdForUserSpec(string notificationId, string userId)
			: base(n => n.Id == notificationId && n.UserId == userId)
		{
			EnableTracking();
		}
	}
}
