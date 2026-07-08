using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Verification.Commands.Reject
{
	public sealed class RejectVerificationCommandValidator : AbstractValidator<RejectVerificationCommand>
	{
		public RejectVerificationCommandValidator()
			=> RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
	}

}
