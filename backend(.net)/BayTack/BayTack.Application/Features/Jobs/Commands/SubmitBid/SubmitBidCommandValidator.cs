using FluentValidation;

namespace BayTack.Application.Features.Jobs.Commands.SubmitBid
{
	public sealed class SubmitBidCommandValidator : AbstractValidator<SubmitBidCommand>
	{
		public SubmitBidCommandValidator()
		{
			RuleFor(x => x.JobId).NotEmpty();
			RuleFor(x => x.ProviderId).NotEmpty();
			RuleFor(x => x.Price).GreaterThan(0);
			RuleFor(x => x.Currency).NotEmpty().MaximumLength(3);
			RuleFor(x => x.DurationInDays).GreaterThan(0);
			RuleFor(x => x.Notes).MaximumLength(2000);
		}
	}
}
