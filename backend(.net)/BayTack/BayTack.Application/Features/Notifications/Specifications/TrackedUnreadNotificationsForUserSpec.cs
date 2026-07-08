using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.UserFeatures;

namespace BayTack.Application.Features.Notifications.Specifications
{
	/// <summary>Tracked (feeds a command): every unread notification for a user.</summary>
	public sealed class TrackedUnreadNotificationsForUserSpec : Specification<Notification>
	{
		public TrackedUnreadNotificationsForUserSpec(string userId)
			: base(n => n.UserId == userId && !n.IsRead)
		{
			EnableTracking();
		}
	}
}
