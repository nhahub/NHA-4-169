using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Users.Command.UpdateUser
{

	public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
	{
		public UpdateUserCommandValidator()
		{
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
			RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
			RuleFor(x => x.Phone).MaximumLength(20);
			RuleFor(x => x.Role).MaximumLength(256);
		}
	}

}
