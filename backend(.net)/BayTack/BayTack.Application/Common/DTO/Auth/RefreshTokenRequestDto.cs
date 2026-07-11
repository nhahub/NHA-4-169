using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Common.DTO.Auth
{
	public record RefreshTokenRequestDto(string AccessToken, string RefreshToken);

}
