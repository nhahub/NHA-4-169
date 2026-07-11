using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Identity.Command.Register
{

	public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
	{
		public RegisterCommandValidator()
		{
			RuleFor(x => x.FullName).NotEmpty().MaximumLength(150);
			RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
			RuleFor(x => x.Password).NotEmpty().MinimumLength(8)
				.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
				.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
				.Matches("[0-9]").WithMessage("Password must contain at least one digit.");
			RuleFor(x => x.Phone).MaximumLength(20);
			RuleFor(x => x.Role)
				.NotEmpty()
				.Must(r => r.Equals("customer", StringComparison.OrdinalIgnoreCase) || r.Equals("provider", StringComparison.OrdinalIgnoreCase))
				.WithMessage("Role must be either 'customer' or 'provider'.");
		}
	}
}
