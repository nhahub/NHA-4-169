using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.ServiceAggregate;

namespace BayTack.Application.Features.Categories.Specifications
{
	/// <summary>Every category, ordered by name — used by GetAllCategories.</summary>
	public sealed class AllCategoriesSpecification : Specification<ServiceCategory>
	{
		public AllCategoriesSpecification() => ApplyOrderBy(c => c.Name);
	}

	/// <summary>Matches a category by exact name (case handled by DB collation) — used for the uniqueness check on create/rename.</summary>
	public sealed class CategoryByNameSpecification : Specification<ServiceCategory>
	{
		public CategoryByNameSpecification(string name) : base(c => c.Name == name) { }
	}
}
