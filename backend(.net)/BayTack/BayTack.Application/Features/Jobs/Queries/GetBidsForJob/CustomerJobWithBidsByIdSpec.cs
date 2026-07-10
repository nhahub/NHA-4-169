using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.JobAggregate;

namespace BayTack.Application.Features.Jobs.Queries.GetBidsForJob
{
	public sealed class CustomerJobWithBidsByIdSpec : Specification<CustomerJob>
	{
		public CustomerJobWithBidsByIdSpec(string jobId) : base(j => j.Id == jobId)
		{
			AddInclude(j => j.Bids);
		}
	}
}
