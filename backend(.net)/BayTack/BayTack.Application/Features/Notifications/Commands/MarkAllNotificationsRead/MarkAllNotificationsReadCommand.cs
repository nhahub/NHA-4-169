using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Notifications.Commands.MarkAllNotificationsRead
{
	public sealed record MarkAllNotificationsReadCommand(string UserId) : ICommand;
}
