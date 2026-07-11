using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Common.DTO.Auth
{

	public record RegisterDto(
		string FirstName,
		string LastName,
		string Email,
		string UserName,
		string Password,
		string ConfirmPassword);

}



