using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.JobAggregate;

namespace BayTack.Application.Features.Jobs.Specifications
{
    /// <summary>
    /// Tracked (feeds a command): a single request (job) scoped to its owner, with offers (bids)
    /// loaded, so a customer can never delete/accept-offer-on another customer's job by guessing an id.
    /// </summary>
    public sealed class TrackedCustomerJobByIdForUserSpec : Specification<CustomerJob>
    {
        public TrackedCustomerJobByIdForUserSpec(string jobId, string customerId)
            : base(j => j.Id == jobId && j.CustomerId == customerId)
        {
            AddInclude(j => j.Bids);
            EnableTracking();
        }
    }
}