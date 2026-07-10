using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.JobAggregate;

namespace BayTack.Application.Features.Jobs.Commands.SubmitBid
{
	public sealed class CustomerJobByIdForBiddingSpec : Specification<CustomerJob>
	{
		public CustomerJobByIdForBiddingSpec(string jobId) : base(j => j.Id == jobId)
		{
			AddInclude(j => j.Bids);
			EnableTracking();
		}
	}
}
