using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.DeletePortfolioItem
{
	public sealed class ProviderProfileByPortfolioItemIdSpec : Specification<ProviderProfile>
	{
		public ProviderProfileByPortfolioItemIdSpec(string itemId) : base(p => p.Portfolio.Any(i => i.Id == itemId))
		{
			AddInclude(p => p.Portfolio);
			EnableTracking();
		}
	}
}
