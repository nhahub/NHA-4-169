using FluentValidation;

namespace BayTack.Application.Features.Preferences.Commands.UpdateMyPreferences
{
    public sealed class UpdateMyPreferencesCommandValidator : AbstractValidator<UpdateMyPreferencesCommand>
    {
        public UpdateMyPreferencesCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Language).NotEmpty().MaximumLength(10);
            RuleFor(x => x.Theme).NotEmpty().MaximumLength(10);
        }
    }
}
