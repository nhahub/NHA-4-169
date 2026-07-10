using FluentValidation;

namespace BayTack.Application.Features.Orders.Commands.DeclineBooking
{
	public sealed class DeclineBookingCommandValidator : AbstractValidator<DeclineBookingCommand>
	{
		public DeclineBookingCommandValidator()
		{
			RuleFor(x => x.OrderId).NotEmpty();
			RuleFor(x => x.ChangedBy).NotEmpty();
		}
	}
}
