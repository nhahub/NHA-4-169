using FluentValidation;

namespace BayTack.Application.Features.Providers.Commands.CreateProviderProfile
{
	public sealed class CreateProviderProfileCommandValidator : AbstractValidator<CreateProviderProfileCommand>
	{
		public CreateProviderProfileCommandValidator()
		{
			RuleFor(x => x.UserId).NotEmpty();
			RuleFor(x => x.ProviderType).IsInEnum();
			RuleFor(x => x.YearsOfExperience).InclusiveBetween(0, 80);
			RuleFor(x => x.Bio).MaximumLength(2000);
		}
	}
}
