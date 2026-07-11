using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Common.DTO.Auth
{
	public record ResetPasswordDto(string Email, string Token, string NewPassword);

}






