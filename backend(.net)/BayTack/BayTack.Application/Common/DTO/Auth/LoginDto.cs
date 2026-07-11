using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Common.DTO.Auth
{
	public record LoginDto(string Email, string Password, string? IpAddress);

}
