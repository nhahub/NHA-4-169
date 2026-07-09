using FluentValidation;

namespace BayTack.Application.Features.Providers.Queries.GetAllPortfolioItems
{
	public sealed class GetAllPortfolioItemsQueryValidator : AbstractValidator<GetAllPortfolioItemsQuery>
	{
		public GetAllPortfolioItemsQueryValidator()
		{
			RuleFor(x => x.ProviderProfileId).NotEmpty();
		}
	}
}
