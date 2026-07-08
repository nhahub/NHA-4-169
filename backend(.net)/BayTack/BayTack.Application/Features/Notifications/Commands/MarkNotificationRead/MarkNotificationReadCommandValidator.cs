using FluentValidation;

namespace BayTack.Application.Features.Notifications.Commands.MarkNotificationRead
{
	public sealed class MarkNotificationReadCommandValidator : AbstractValidator<MarkNotificationReadCommand>
	{
		public MarkNotificationReadCommandValidator()
		{
			RuleFor(x => x.UserId).NotEmpty();
			RuleFor(x => x.NotificationId).NotEmpty();
		}
	}
}
