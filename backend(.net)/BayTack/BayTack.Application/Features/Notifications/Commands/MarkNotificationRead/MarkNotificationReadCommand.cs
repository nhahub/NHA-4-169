using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Notifications.Common;

namespace BayTack.Application.Features.Notifications.Commands.MarkNotificationRead
{
	public sealed record MarkNotificationReadCommand(string UserId, string NotificationId) : ICommand<NotificationResponse>;
}
