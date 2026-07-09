using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.UserFeatures;

namespace BayTack.Application.Features.SavedServices.Specifications
{
    /// <summary>Read-only: every saved-service bookmark for a customer.</summary>
    public sealed class SavedServicesForUserSpec : Specification<SavedService>
    {
        public SavedServicesForUserSpec(string customerId) : base(s => s.CustomerId == customerId)
        {
            ApplyOrderByDescending(s => s.CreatedAt);
        }
    }
}