using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Users.Command.DeleteUser
{

	public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
	{
		public DeleteUserCommandValidator()
		{
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.DeletedBy).NotEmpty();
			RuleFor(x => x.Reason).MaximumLength(500);
		}
	}

}
