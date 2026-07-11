using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Common.DTO.Auth
{
	public record AuthResponseDto(
	string UserId,
	string Email,
	string UserName,
	string FullName,
	string AccessToken,
	DateTime AccessTokenExpiration,
	string RefreshToken,
	IEnumerable<string> Roles);
}


