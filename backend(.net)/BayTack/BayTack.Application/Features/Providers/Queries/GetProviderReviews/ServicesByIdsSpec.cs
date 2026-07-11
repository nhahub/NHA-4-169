using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.ServiceAggregate;

namespace BayTack.Application.Features.Providers.Queries.GetProviderReviews
{
	public sealed class ServicesByIdsSpec : Specification<Service>
	{
		public ServicesByIdsSpec(List<string> serviceIds) : base(s => serviceIds.Contains(s.Id))
		{
		}
	}
}
