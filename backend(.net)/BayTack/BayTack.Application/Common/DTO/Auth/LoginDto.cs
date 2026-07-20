using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Common.DTO.Auth
{
	public record LoginDto(string Identifier, string Password, string? IpAddress);

}
