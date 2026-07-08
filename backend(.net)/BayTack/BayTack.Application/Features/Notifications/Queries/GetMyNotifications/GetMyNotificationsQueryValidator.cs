using FluentValidation;

namespace BayTack.Application.Features.Notifications.Queries.GetMyNotifications
{
	public sealed class GetMyNotificationsQueryValidator : AbstractValidator<GetMyNotificationsQuery>
	{
		public GetMyNotificationsQueryValidator()
		{
			RuleFor(x => x.UserId).NotEmpty();
		}
	}
}
