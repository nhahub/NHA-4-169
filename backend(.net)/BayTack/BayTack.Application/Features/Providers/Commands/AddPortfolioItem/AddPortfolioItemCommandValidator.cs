using FluentValidation;

namespace BayTack.Application.Features.Providers.Commands.AddPortfolioItem;

public sealed class AddPortfolioItemCommandValidator : AbstractValidator<AddPortfolioItemCommand>
{
    public AddPortfolioItemCommandValidator()
    {
        RuleFor(c => c.ProviderProfileId).NotEmpty();
        RuleFor(c => c.Title).NotEmpty().MaximumLength(200);
        RuleFor(c => c.Description).MaximumLength(2000);
        RuleFor(c => c.ImageUrl).MaximumLength(1000);
    }
}
