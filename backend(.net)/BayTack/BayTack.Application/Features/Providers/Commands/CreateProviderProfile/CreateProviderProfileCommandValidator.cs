using FluentValidation;

namespace BayTack.Application.Features.Providers.Commands.CreateProviderProfile;

public sealed class CreateProviderProfileCommandValidator : AbstractValidator<CreateProviderProfileCommand>
{
    public CreateProviderProfileCommandValidator()
    {
        RuleFor(c => c.UserId).NotEmpty();
        RuleFor(c => c.ProviderType).IsInEnum();
        RuleFor(c => c.YearsOfExperience).InclusiveBetween(0, 80);
        RuleFor(c => c.Bio).MaximumLength(2000);
    }
}
