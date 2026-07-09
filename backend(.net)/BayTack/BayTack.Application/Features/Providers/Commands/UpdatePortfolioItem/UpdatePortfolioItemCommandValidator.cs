using FluentValidation;

namespace BayTack.Application.Features.Providers.Commands.UpdatePortfolioItem
{
	public sealed class UpdatePortfolioItemCommandValidator : AbstractValidator<UpdatePortfolioItemCommand>
	{
		public UpdatePortfolioItemCommandValidator()
		{
			RuleFor(x => x.ItemId).NotEmpty();
			RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
			RuleFor(x => x.Description).MaximumLength(2000);
			RuleFor(x => x.ImageUrl).MaximumLength(1000);
		}
	}
}
