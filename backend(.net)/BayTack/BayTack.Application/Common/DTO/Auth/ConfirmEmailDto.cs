using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Common.DTO.Auth
{
	public record ConfirmEmailDto(string UserId, string Token, string? ipAddress);

}
