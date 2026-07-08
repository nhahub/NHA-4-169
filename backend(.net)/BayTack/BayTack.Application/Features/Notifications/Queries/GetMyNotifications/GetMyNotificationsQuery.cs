using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Notifications.Common;

namespace BayTack.Application.Features.Notifications.Queries.GetMyNotifications
{
	public sealed record GetMyNotificationsQuery(string UserId) : IQuery<List<NotificationResponse>>;
}
