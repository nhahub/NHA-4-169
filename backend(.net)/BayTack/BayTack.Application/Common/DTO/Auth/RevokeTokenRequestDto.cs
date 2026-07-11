
using System.Net;

namespace BayTack.Application.Common.DTO.Auth
{
	public record RevokeTokenRequestDto(string RefreshToken, string? IpAddress);

}
