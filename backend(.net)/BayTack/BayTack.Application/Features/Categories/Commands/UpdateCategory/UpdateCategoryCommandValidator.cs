using FluentValidation;

namespace BayTack.Application.Features.Categories.Commands.UpdateCategory
{
	public sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
	{
		public UpdateCategoryCommandValidator()
		{
			RuleFor(x => x.Id).NotEmpty();
			RuleFor(x => x.Name).MaximumLength(150);
			RuleFor(x => x.Icon).MaximumLength(100);
			RuleFor(x => x.Description).MaximumLength(500);
		}
	}
}
