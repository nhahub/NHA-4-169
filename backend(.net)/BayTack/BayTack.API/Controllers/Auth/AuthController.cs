using BayTack.API.Extensions;
using BayTack.Application.Features.Identity.Command.Login;
using BayTack.Application.Features.Identity.Command.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Auth
{
	[AllowAnonymous] 
	public class AuthController : ApiController
	{
		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginCommand command)
		{
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			if (response.IsSuccess && response.Data is not null)
			{
				var accessToken = response.Data.AccessToken;
				var refreshToken = response.Data.RefreshToken;
				var accessTokenExpiry = response.Data.AccessTokenExpiration;

				// Access Token
				Response.Cookies.Append("accessToken", accessToken, new CookieOptions
				{
					HttpOnly = true,
					Secure = false,
					SameSite = SameSiteMode.Strict,
					Expires = accessTokenExpiry
				});

				// Refresh Token
				Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
				{
					HttpOnly = true,
					Secure = false,
					SameSite = SameSiteMode.Strict,
					Expires = DateTime.UtcNow.AddDays(7)
				});
			}
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterCommand command)
		{
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			if (response.IsSuccess && response.Data is not null)
			{
				var accessToken = response.Data.AccessToken;
				var refreshToken = response.Data.RefreshToken;
				var accessTokenExpiry = response.Data.AccessTokenExpiration;

				// Access Token
				Response.Cookies.Append("accessToken", accessToken, new CookieOptions
				{
					HttpOnly = true,
					Secure = false,
					SameSite = SameSiteMode.Strict,
					Expires = accessTokenExpiry
				});

				// Refresh Token
				Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
				{
					HttpOnly = true,
					Secure = false,
					SameSite = SameSiteMode.Strict,
					Expires = DateTime.UtcNow.AddDays(7)
				});
			}
			return StatusCode(response.StatusCode, response);
		}

		//[HttpPost("change-password")]
		//[Authorize] 
		//public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
		//{
		//	// الـ userId يفضل يقرأ من الـ User.Claims وليس من الـ Parameter لحماية البيانات
		//	var userId = CurrentUserId ?? throw new InvalidOperationException("User ID is required.");

		//	var result = await Sender.Send(new ChangePasswordCommand(userId, currentPassword, newPassword));
		//	var response = result.ToApiResponse<object>();
		//	return StatusCode(response.StatusCode, response);
		//}

		//[HttpPost("refresh-token")]
		//// متاح للجميع لأن الـ Access Token القديم بيكون انتهت صلاحيته والـ Handler هيتأكد من الـ Refresh Token
		//public async Task<IActionResult> RefreshToken(RefreshTokenCommand command)
		//{
		//	var result = await Sender.Send(command);
		//	var response = result.ToApiResponse();
		//	return StatusCode(response.StatusCode, response);
		//}
	}
}
