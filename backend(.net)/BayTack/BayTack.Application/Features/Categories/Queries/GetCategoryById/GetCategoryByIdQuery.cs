using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Categories.Queries.GetCategoryById
{
	public sealed record GetCategoryByIdQuery(string Id ) : IQuery<CategoryResponse>;
}
