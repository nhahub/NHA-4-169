using FluentValidation;

namespace BayTack.Application.Features.Orders.Commands.CreateOrder
{
	public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
	{
		private static readonly string[] AllowedTiers = { "basic", "standard", "premium" };

		public CreateOrderCommandValidator()
		{
			RuleFor(x => x.CustomerId).NotEmpty();
			RuleFor(x => x.ServiceId).NotEmpty();

			RuleFor(x => x.Tier)
				.Must(t => string.IsNullOrWhiteSpace(t) || AllowedTiers.Contains(t.ToLowerInvariant()))
				.WithMessage("Tier must be one of: basic, standard, premium.");
		}
	}
}
