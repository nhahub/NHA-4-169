using FluentValidation;

namespace BayTack.Application.Features.Orders.Commands.CancelOrder
{
	public sealed class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
	{
		public CancelOrderCommandValidator()
		{
			RuleFor(x => x.CustomerId).NotEmpty();
			RuleFor(x => x.OrderId).NotEmpty();
		}
	}
}
