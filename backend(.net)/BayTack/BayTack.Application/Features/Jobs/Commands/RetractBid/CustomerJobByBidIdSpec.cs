using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.JobAggregate;

namespace BayTack.Application.Features.Jobs.Commands.RetractBid
{
	public sealed class CustomerJobByBidIdSpec : Specification<CustomerJob>
	{
		public CustomerJobByBidIdSpec(string bidId) : base(j => j.Bids.Any(b => b.Id == bidId))
		{
			AddInclude(j => j.Bids);
			EnableTracking();
		}
	}
}
