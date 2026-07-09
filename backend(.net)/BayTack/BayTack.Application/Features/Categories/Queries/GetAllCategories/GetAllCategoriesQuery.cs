using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Categories.Queries.GetAllCategories
{
	public sealed record GetAllCategoriesQuery : IQuery<List<CategoryResponse>>;
}
