using FluentValidation;

namespace BayTack.Application.Features.SavedServices.Commands.RemoveSavedService
{
    public sealed class RemoveSavedServiceCommandValidator : AbstractValidator<RemoveSavedServiceCommand>
    {
        public RemoveSavedServiceCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.ServiceId).NotEmpty();
        }
    }
}