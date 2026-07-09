using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.JobAggregate;

namespace BayTack.Application.Features.Jobs.Specifications
{
    /// <summary>Read-only: a single request (job), scoped to its owner, with offers (bids) loaded.</summary>
    public sealed class CustomerJobByIdForUserSpec : Specification<CustomerJob>
    {
        public CustomerJobByIdForUserSpec(string jobId, string customerId)
            : base(j => j.Id == jobId && j.CustomerId == customerId)
        {
            AddInclude(j => j.Bids);
        }
    }
}