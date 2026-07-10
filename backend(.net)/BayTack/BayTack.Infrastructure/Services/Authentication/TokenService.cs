using BayTack.Application.Abstractions.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BayTack.Infrastructure.Services.Authentication
{
	public class TokenService : ITokenService
	{
		private readonly TokenSettings _tokenSettings;

		public TokenService(IOptions<TokenSettings> jwtSettings)
		{
			_tokenSettings = jwtSettings.Value;
		}


		public GeneratedToken GenerateAccessToken(string userId, string email, string FirstName, string LastName, IEnumerable<string> roles, IEnumerable<string> permissions)
		{
			var claims = new List<Claim>
			{
				new(JwtRegisteredClaimNames.Sub,   userId),
				new(JwtRegisteredClaimNames.Email, email),
				new(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
				new("firstName", FirstName),
				new("lastName",  LastName),
			};
			foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

			foreach (var perm in permissions) claims.Add(new Claim("Permission", perm));

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _tokenSettings.Issuer,
				audience: _tokenSettings.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_tokenSettings.ExpiryInMinutes),
				signingCredentials: creds);

			return new GeneratedToken(new JwtSecurityTokenHandler().WriteToken(token), Guid.NewGuid().ToString(), DateTime.UtcNow.AddMinutes(_tokenSettings.ExpiryInMinutes));
		}

		public string GenerateRefreshTokenString()
		{
			var randomNumber = new byte[64];

			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);

			return Convert.ToBase64String(randomNumber);
		}


		public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
		{
			var parameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = false,
				ValidateIssuerSigningKey = true,
				ValidIssuer = _tokenSettings.Issuer,
				ValidAudience = _tokenSettings.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.SecretKey))
			};

			try
			{
				var principal = new JwtSecurityTokenHandler().ValidateToken(token, parameters, out var secToken);
				if (secToken is not JwtSecurityToken jwt ||
					!jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
						StringComparison.InvariantCultureIgnoreCase))
					return null;

				return principal;
			}
			catch { return null; }
		}


	}
}
