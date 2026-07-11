using BayTack.Application.Common.Specifications;
using BayTack.Infrastructure.Identity;

namespace BayTack.Infrastructure.Services.Authentication
{
	public sealed class RefreshTokenByValueSpec : Specification<RefreshToken>
	{
		public RefreshTokenByValueSpec(string value)
			: base(x => x.Value == value)
		{
			EnableTracking();
		}
	}
}
