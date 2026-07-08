using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.UserFeatures;

namespace BayTack.Application.Features.Notifications.Specifications
{
	/// <summary>Read-only: every notification belonging to a user, newest first.</summary>
	public sealed class NotificationsForUserSpec : Specification<Notification>
	{
		public NotificationsForUserSpec(string userId) : base(n => n.UserId == userId)
		{
			ApplyOrderByDescending(n => n.CreatedAt);
		}
	}
}
