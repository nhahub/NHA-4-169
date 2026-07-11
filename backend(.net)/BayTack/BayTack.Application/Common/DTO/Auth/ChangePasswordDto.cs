using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Common.DTO.Auth
{
	public record ChangePasswordDto(string CurrentPassword, string NewPassword, string ConfirmPassword);
}

