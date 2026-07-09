using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.CreateProviderProfile;

public sealed class ProviderProfileByUserIdSpec(string userId) : Specification<ProviderProfile>(p => p.UserId == userId)
{
}
