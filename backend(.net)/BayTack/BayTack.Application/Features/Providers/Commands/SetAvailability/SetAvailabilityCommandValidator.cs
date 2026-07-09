using FluentValidation;

namespace BayTack.Application.Features.Providers.Commands.SetAvailability;

public sealed class SetAvailabilityCommandValidator : AbstractValidator<SetAvailabilityCommand>
{
    public SetAvailabilityCommandValidator()
    {
        RuleFor(c => c.ProviderProfileId).NotEmpty();
        RuleFor(c => c.DayOfWeek).IsInEnum();
        RuleFor(c => c)
            .Must(c => c.StartTime < c.EndTime)
            .WithMessage("Start time must be before end time.");
    }
}
