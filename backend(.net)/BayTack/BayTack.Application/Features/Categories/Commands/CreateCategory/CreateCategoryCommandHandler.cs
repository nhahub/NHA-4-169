using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Categories.Specifications;
using BayTack.Domain.Entities.ServiceAggregate;

namespace BayTack.Application.Features.Categories.Commands.CreateCategory
{
	public sealed class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryResponse>
	{
		private readonly IRepository<ServiceCategory, string> _categories;

		public CreateCategoryCommandHandler(IRepository<ServiceCategory, string> categories )
		{
			_categories = categories;
		}

		public async Task<Result<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
		{
			var exists = await _categories.AnyAsync(new CategoryByNameSpecification(request.Name), cancellationToken);
			if (exists)
				return Result<CategoryResponse>.Failure($"Category '{request.Name}' already exists");

			var category = ServiceCategory.Create(request.Name, request.Icon, request.Description);
			_categories.Add(category);

			return Result<CategoryResponse>.Success(
				new CategoryResponse(category.Id, category.Name, request.Icon, category.Description, category.IsActive, category.CreatedAt));
		}
	}
}

