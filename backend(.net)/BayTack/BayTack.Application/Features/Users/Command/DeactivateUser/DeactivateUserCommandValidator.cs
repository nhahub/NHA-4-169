using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Users.Command.DeactivateUser
{

	public sealed class DeactivateUserCommandValidator : AbstractValidator<DeactivateUserCommand>
	{
		public DeactivateUserCommandValidator()
		{
			RuleFor(x => x.Id).NotEmpty();
		}
	}

}
