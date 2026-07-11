using FluentValidation;

namespace BayTack.Application.Features.Providers.Commands.RespondToReview
{
	public sealed class RespondToReviewCommandValidator : AbstractValidator<RespondToReviewCommand>
	{
		public RespondToReviewCommandValidator()
		{
			RuleFor(x => x.ReviewId).NotEmpty();
			RuleFor(x => x.ResponseText).NotEmpty().MaximumLength(2000);
		}
	}
}
