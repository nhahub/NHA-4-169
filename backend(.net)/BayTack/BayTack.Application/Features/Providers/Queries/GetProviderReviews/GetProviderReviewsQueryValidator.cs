using FluentValidation;

namespace BayTack.Application.Features.Providers.Queries.GetProviderReviews
{
	public sealed class GetProviderReviewsQueryValidator : AbstractValidator<GetProviderReviewsQuery>
	{
		public GetProviderReviewsQueryValidator()
		{
			RuleFor(x => x.ProviderId).NotEmpty();
			RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
			RuleFor(x => x.Limit).InclusiveBetween(1, 100);
		}
	}
}
