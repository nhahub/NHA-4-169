using FluentValidation;

namespace BayTack.Application.Features.SavedServices.Commands.AddSavedService
{
    public sealed class AddSavedServiceCommandValidator : AbstractValidator<AddSavedServiceCommand>
    {
        public AddSavedServiceCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.ServiceId).NotEmpty();
        }
    }
}