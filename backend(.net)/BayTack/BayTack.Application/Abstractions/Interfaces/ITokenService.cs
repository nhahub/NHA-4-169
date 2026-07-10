using System.Security.Claims;

namespace BayTack.Application.Abstractions.Interfaces
{

	public record GeneratedToken(string Token, string JwtId, DateTime ExpiresAt);

	public interface ITokenService
	{
		/// <summary>Builds a signed JWT access token embedding roles and permissions as claims.</summary>
		GeneratedToken GenerateAccessToken(string userId, string email, string FirstName, string LastName, IEnumerable<string> roles, IEnumerable<string> permissions);

		/// <summary>Generates a cryptographically random refresh token string.</summary>
		string GenerateRefreshTokenString();

		/// <summary>Extracts the ClaimsPrincipal from an expired (but validly signed) access token.</summary>
		ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

	}
}
