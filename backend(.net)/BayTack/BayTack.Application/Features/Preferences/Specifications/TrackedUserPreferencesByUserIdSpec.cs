using BayTack.Application.Common.Specifications;
using BayTack.Domain.Entities.UserFeatures;

namespace BayTack.Application.Features.Preferences.Specifications
{
    public sealed class TrackedUserPreferencesByUserIdSpec : Specification<UserPreferences>
    {
        public TrackedUserPreferencesByUserIdSpec(string userId) : base(p => p.UserId == userId)
        {
            EnableTracking();
        }
    }
}
