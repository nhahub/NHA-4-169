using BayTack.API.Extensions;
using BayTack.Application.Features.Identity.Command.Login;
using BayTack.Application.Features.Identity.Command.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Auth
{
	public class AuthController : ApiController 
	{
		//https://localhost:7212/api/auth/login
		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginCommand command)
		{
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}


		//https://localhost:7212/api/auth/register
		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterCommand command)
		{
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}


		////https://localhost:7212/api/auth/change-password
		//[HttpPost("change-password")]
		//public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
		//{

		//	var result = await Sender.Send(new ChangePasswordCommand(userId, currentPassword, newPassword));

		//	var response = result.ToApiResponse<object>();

		//	return StatusCode(response.StatusCode, response);
		//}



		//// https://localhost:7212/api/auth/refresh-token
		//[HttpPost("refresh-token")]
		//[AllowAnonymous]
		//public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
		//{
		//	var result = await Sender.Send(command);

		//	var response = result.ToApiResponse();

		//	return StatusCode(response.StatusCode, response);
		//}

	}
}
