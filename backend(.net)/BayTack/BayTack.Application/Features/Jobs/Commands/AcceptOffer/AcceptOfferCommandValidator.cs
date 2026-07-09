using FluentValidation;

namespace BayTack.Application.Features.Jobs.Commands.AcceptOffer
{
    public sealed class AcceptOfferCommandValidator : AbstractValidator<AcceptOfferCommand>
    {
        public AcceptOfferCommandValidator()
        {
            RuleFor(x => x.JobId).NotEmpty();
            RuleFor(x => x.OfferId).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
        }
    }
}