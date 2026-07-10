using FluentValidation;

namespace BayTack.Application.Features.Orders.Commands.AcceptBooking
{
	public sealed class AcceptBookingCommandValidator : AbstractValidator<AcceptBookingCommand>
	{
		public AcceptBookingCommandValidator()
		{
			RuleFor(x => x.OrderId).NotEmpty();
			RuleFor(x => x.ChangedBy).NotEmpty();
		}
	}
}
