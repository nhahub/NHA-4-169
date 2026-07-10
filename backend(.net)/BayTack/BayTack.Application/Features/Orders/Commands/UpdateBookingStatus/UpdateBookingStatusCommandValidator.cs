using FluentValidation;

namespace BayTack.Application.Features.Orders.Commands.UpdateBookingStatus
{
	public sealed class UpdateBookingStatusCommandValidator : AbstractValidator<UpdateBookingStatusCommand>
	{
		public UpdateBookingStatusCommandValidator()
		{
			RuleFor(x => x.OrderId).NotEmpty();
			RuleFor(x => x.NewStatus).IsInEnum();
			RuleFor(x => x.ChangedBy).NotEmpty();
		}
	}
}
