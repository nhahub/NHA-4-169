using FluentValidation;

namespace BayTack.Application.Features.Providers.Queries.GetProviderReviewStats
{
	public sealed class GetProviderReviewStatsQueryValidator : AbstractValidator<GetProviderReviewStatsQuery>
	{
		public GetProviderReviewStatsQueryValidator()
		{
			RuleFor(x => x.ProviderId).NotEmpty();
		}
	}
}
