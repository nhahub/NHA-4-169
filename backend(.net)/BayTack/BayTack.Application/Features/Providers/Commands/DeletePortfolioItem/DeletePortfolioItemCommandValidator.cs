using FluentValidation;

namespace BayTack.Application.Features.Providers.Commands.DeletePortfolioItem
{
	public sealed class DeletePortfolioItemCommandValidator : AbstractValidator<DeletePortfolioItemCommand>
	{
		public DeletePortfolioItemCommandValidator()
		{
			RuleFor(x => x.ItemId).NotEmpty();
		}
	}
}
