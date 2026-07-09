using FluentValidation;

namespace BayTack.Application.Features.Jobs.Commands.PlaceBid
{
    public sealed class PlaceBidCommandValidator : AbstractValidator<PlaceBidCommand>
    {
        public PlaceBidCommandValidator()
        {
            RuleFor(x => x.JobId).NotEmpty();
            RuleFor(x => x.ProviderId).NotEmpty();
            RuleFor(x => x.ProposedPrice).GreaterThan(0);
            RuleFor(x => x.Currency).NotEmpty().Length(3);
            RuleFor(x => x.DurationInDays).GreaterThan(0).LessThanOrEqualTo(365);
            RuleFor(x => x.Notes).MaximumLength(2000);
        }
    }
}