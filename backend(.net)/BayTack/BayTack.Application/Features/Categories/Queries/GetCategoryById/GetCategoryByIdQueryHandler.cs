using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ServiceAggregate;

namespace BayTack.Application.Features.Categories.Queries.GetCategoryById
{
	public sealed class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryResponse>
	{
		private readonly IRepository<ServiceCategory, string> _categories;

		public GetCategoryByIdQueryHandler(IRepository<ServiceCategory, string> categories) => _categories = categories;

		public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
		{
			var category = await _categories.GetByIdAsync(request.Id, cancellationToken);
			if (category is null)
				return Result<CategoryResponse>.NotFound($"Category '{request.Id}' not found");

			return Result<CategoryResponse>.Success(
				new CategoryResponse(category.Id, category.Name, category.Icon, category.Description, category.IsActive, category.CreatedAt));
		}
	}
}
