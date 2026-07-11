using BayTack.Application.Common.Specifications;
using BayTack.Infrastructure.Identity;
 
namespace BayTack.Infrastructure.Services.Authentication
{
	public sealed class ActiveRefreshTokensForUserSpec : Specification<RefreshToken>
	{
		public ActiveRefreshTokensForUserSpec(string userId)
			: base(x =>
				x.UserId == userId &&
				!x.IsRevoked &&
				!x.IsUsed &&
				x.ExpiryDate > DateTime.UtcNow)
		{
			EnableTracking();
		}
	}
}
