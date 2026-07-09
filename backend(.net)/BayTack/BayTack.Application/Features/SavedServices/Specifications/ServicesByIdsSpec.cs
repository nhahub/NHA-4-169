using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.ServiceAggregate;

namespace BayTack.Application.Features.SavedServices.Specifications
{
    /// <summary>Read-only: the full Service entities for a set of ids, used to hydrate saved-service ids
    /// into full Service objects (a spec can only target one aggregate, so this is a second lookup
    /// rather than a cross-aggregate join in the SavedService spec).</summary>
    public sealed class ServicesByIdsSpec : Specification<Service>
    {
        public ServicesByIdsSpec(IReadOnlyCollection<string> ids) : base(s => ids.Contains(s.Id))
        {
        }
    }
}