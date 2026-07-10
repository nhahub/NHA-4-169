using FluentValidation;

namespace BayTack.Application.Features.Orders.Commands.CompleteBooking
{
	public sealed class CompleteBookingCommandValidator : AbstractValidator<CompleteBookingCommand>
	{
		public CompleteBookingCommandValidator()
		{
			RuleFor(x => x.OrderId).NotEmpty();
			RuleFor(x => x.ChangedBy).NotEmpty();
		}
	}
}
