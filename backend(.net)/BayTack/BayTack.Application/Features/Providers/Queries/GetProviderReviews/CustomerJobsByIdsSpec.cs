using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.JobAggregate;

namespace BayTack.Application.Features.Providers.Queries.GetProviderReviews
{
	public sealed class CustomerJobsByIdsSpec : Specification<CustomerJob>
	{
		public CustomerJobsByIdsSpec(List<string> jobIds) : base(j => jobIds.Contains(j.Id))
		{
		}
	}
}
