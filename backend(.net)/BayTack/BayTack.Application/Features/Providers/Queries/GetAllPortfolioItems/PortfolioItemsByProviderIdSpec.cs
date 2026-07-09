using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Queries.GetAllPortfolioItems
{
	public sealed class PortfolioItemsByProviderIdSpec : Specification<ProviderPortfolioItem>
	{
		public PortfolioItemsByProviderIdSpec(string providerProfileId)
			: base(p => p.ProviderProfileId == providerProfileId)
		{
		}
	}
}
