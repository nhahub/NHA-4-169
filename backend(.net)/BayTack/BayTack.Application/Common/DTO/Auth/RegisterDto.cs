using System;

namespace BayTack.Application.Common.DTO.Auth
{
	public record RegisterDto(
		string FullName,
		string Email,
		string Password,
		string? PhoneNumber,
		string Role,
		string? IpAddress);

}


