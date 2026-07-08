using FluentValidation;

namespace BayTack.Application.Features.Notifications.Commands.MarkAllNotificationsRead
{
	public sealed class MarkAllNotificationsReadCommandValidator : AbstractValidator<MarkAllNotificationsReadCommand>
	{
		public MarkAllNotificationsReadCommandValidator()
		{
			RuleFor(x => x.UserId).NotEmpty();
		}
	}
}
