using FluentValidation;

namespace BayTack.Application.Features.Categories.Commands.CreateCategory
{
	public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
	{
		public CreateCategoryCommandValidator()
		{
			RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
			RuleFor(x => x.Icon).MaximumLength(100);
			RuleFor(x => x.Description).MaximumLength(500);
		}
	}
}
