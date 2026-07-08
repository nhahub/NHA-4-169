using FluentValidation;

namespace BayTack.Application.Features.Providers.Commands.AddPortfolioItem
{
	public sealed class AddPortfolioItemCommandValidator : AbstractValidator<AddPortfolioItemCommand>
	{
		public AddPortfolioItemCommandValidator()
		{
			RuleFor(x => x.ProviderProfileId).NotEmpty();
			RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
			RuleFor(x => x.Description).MaximumLength(2000);
			RuleFor(x => x.ImageUrl).MaximumLength(1000);
		}
	}
}
