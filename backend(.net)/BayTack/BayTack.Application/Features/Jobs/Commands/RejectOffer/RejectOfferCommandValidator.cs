using FluentValidation;

namespace BayTack.Application.Features.Jobs.Commands.RejectOffer
{
    public sealed class RejectOfferCommandValidator : AbstractValidator<RejectOfferCommand>
    {
        public RejectOfferCommandValidator()
        {
            RuleFor(x => x.JobId).NotEmpty();
            RuleFor(x => x.OfferId).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
        }
    }
}
