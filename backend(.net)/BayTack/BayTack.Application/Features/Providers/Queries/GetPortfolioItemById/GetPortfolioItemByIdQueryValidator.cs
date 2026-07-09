using FluentValidation;

namespace BayTack.Application.Features.Providers.Queries.GetPortfolioItemById
{
	public sealed class GetPortfolioItemByIdQueryValidator : AbstractValidator<GetPortfolioItemByIdQuery>
	{
		public GetPortfolioItemByIdQueryValidator()
		{
			RuleFor(x => x.ItemId).NotEmpty();
		}
	}
}
