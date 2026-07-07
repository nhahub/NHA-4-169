using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.CreateProviderProfile
{
	public sealed class ProviderProfileByUserIdSpec : Specification<ProviderProfile>
	{
		public ProviderProfileByUserIdSpec(string userId) : base(p => p.UserId == userId)
		{
		}
	}
}
