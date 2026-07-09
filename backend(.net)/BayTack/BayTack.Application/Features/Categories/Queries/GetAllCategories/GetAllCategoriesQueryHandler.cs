using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Categories.Specifications;
using BayTack.Domain.Entities.ServiceAggregate;

namespace BayTack.Application.Features.Categories.Queries.GetAllCategories
{
	public sealed class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, List<CategoryResponse>>
	{
		private readonly IRepository<ServiceCategory, string> _categories;

		public GetAllCategoriesQueryHandler(IRepository<ServiceCategory, string> categories) => _categories = categories;

		public async Task<Result<List<CategoryResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
		{
			var categories = await _categories.ListAsync(new AllCategoriesSpecification(), cancellationToken);

			var response = categories
				.Select(c => new CategoryResponse(c.Id, c.Name, c.Icon, c.Description, c.IsActive, c.CreatedAt))
				.ToList();

			return Result<List<CategoryResponse>>.Success(response);
		}
	}
}
