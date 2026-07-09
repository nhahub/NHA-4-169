using FluentValidation;

namespace BayTack.Application.Features.Providers.Commands.UpdateProviderBio;

public sealed class UpdateProviderBioCommandValidator : AbstractValidator<UpdateProviderBioCommand>
{
    public UpdateProviderBioCommandValidator()
    {
        RuleFor(c => c.ProviderProfileId).NotEmpty();
        RuleFor(c => c.Bio).NotEmpty().MaximumLength(2000);
        RuleFor(c => c.UpdatedBy).NotEmpty();
    }
}
