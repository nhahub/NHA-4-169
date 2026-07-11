using FluentValidation;

namespace BayTack.Application.Features.Users.Command.CreateUser
{
	public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
	{
		public CreateUserCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
			RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
			RuleFor(x => x.Phone).MaximumLength(20);
			RuleFor(x => x.Role).NotEmpty().MaximumLength(256);
			
		}
	}
}
