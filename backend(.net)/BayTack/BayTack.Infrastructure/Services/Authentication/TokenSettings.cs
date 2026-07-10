
namespace BayTack.Infrastructure.Services.Authentication
{
	public class TokenSettings
	{
		public string Issuer { get; set; } = string.Empty;
		public string Audience { get; set; } = string.Empty;
		public string SecretKey { get; set; } = string.Empty;
		public int ExpiryInMinutes { get; set; }
	}
}
