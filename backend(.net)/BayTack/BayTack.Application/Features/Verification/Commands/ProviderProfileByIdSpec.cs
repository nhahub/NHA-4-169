using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Verification.Commands
{
	public sealed class ProviderProfileByIdSpec : Specification<ProviderProfile>
	{
		public ProviderProfileByIdSpec(string providerProfileId, bool forUpdate = false)
		{
			AddCriteria(p => p.Id == providerProfileId);
			AddInclude(p => p.Documents);

			if (forUpdate)
				EnableTracking();
		}
	}
}
