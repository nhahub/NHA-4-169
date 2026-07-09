using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.UserFeatures;

namespace BayTack.Application.Features.SavedServices.Specifications
{
    /// <summary>Tracked (feeds a command): the bookmark row (if any) for a given customer + service pair.
    /// Used by both Add (to avoid duplicates) and Remove.</summary>
    public sealed class SavedServiceByCustomerAndServiceSpec : Specification<SavedService>
    {
        public SavedServiceByCustomerAndServiceSpec(string customerId, string serviceId)
            : base(s => s.CustomerId == customerId && s.ServiceId == serviceId)
        {
            EnableTracking();
        }
    }
}