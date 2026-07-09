using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.JobAggregate;

namespace BayTack.Application.Features.Jobs.Specifications
{
    /// <summary>Read-only: every request (job) posted by a customer, with its offers (bids), newest first.</summary>
    public sealed class CustomerJobsForUserSpec : Specification<CustomerJob>
    {
        public CustomerJobsForUserSpec(string customerId) : base(j => j.CustomerId == customerId)
        {
            AddInclude(j => j.Bids);
            ApplyOrderByDescending(j => j.CreatedAt);
        }
    }
}