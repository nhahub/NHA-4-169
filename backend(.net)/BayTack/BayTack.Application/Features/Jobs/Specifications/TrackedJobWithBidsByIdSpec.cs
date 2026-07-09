using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.JobAggregate;

namespace BayTack.Application.Features.Jobs.Specifications
{
    /// <summary>Tracked (feeds a command): a single job with its bids loaded, for PlaceBid/AcceptOffer.</summary>
    public sealed class TrackedJobWithBidsByIdSpec : Specification<CustomerJob>
    {
        public TrackedJobWithBidsByIdSpec(string jobId) : base(j => j.Id == jobId)
        {
            AddInclude(j => j.Bids);
            EnableTracking();
        }
    }
}