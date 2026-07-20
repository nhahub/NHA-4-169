using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.Location;

namespace BayTack.Application.Features.Locations.Specifications
{
    /// <summary>Read-only: every city with its areas loaded, for the /cities lookup used by
    /// Post-a-Request (customer) and workshop address (provider) location pickers.</summary>
    public sealed class AllCitiesWithAreasSpec : Specification<City>
    {
        public AllCitiesWithAreasSpec()
        {
            AddInclude(c => c.Areas);
            ApplyOrderBy(c => c.Name);
        }
    }
}
