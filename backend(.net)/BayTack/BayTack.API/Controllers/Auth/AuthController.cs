using BayTack.Application.Common.DTO;
using BayTack.Application.Common.DTO.Auth;
using BayTack.Application.Features.Identity.Command.ChangePassword;
using BayTack.Application.Features.Identity.Command.Login;
using BayTack.Application.Features.Identity.Command.Logout;
using BayTack.Application.Features.Identity.Command.RefreshToken;
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
				SetAuthCookies(response.Data);

			return StatusCode(response.StatusCode, response);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(RegisterCommand command)
		{
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			if (response.IsSuccess && response.Data is not null)
				SetAuthCookies(response.Data);

			return StatusCode(response.StatusCode, response);
		}

		[HttpPost("change-password")]
		[Authorize]
		public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
		{
			// الـ userId يفضل يقرأ من الـ User.Claims وليس من الـ Parameter لحماية البيانات
			var userId = CurrentUserId ?? throw new InvalidOperationException("User ID is required.");

			var result = await Sender.Send(new ChangePasswordCommand(userId, request.CurrentPassword, request.NewPassword));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost("refresh-token")]
		[AllowAnonymous]
		// متاح للجميع لأن الـ Access Token القديم بيكون انتهت صلاحيته والـ Handler هيتأكد من الـ Refresh Token
		public async Task<IActionResult> RefreshToken()
		{
			var refreshToken = Request.Cookies["refreshToken"];
			if (string.IsNullOrEmpty(refreshToken))
			{
				return StatusCode(401, new ApiResponse<object> { IsSuccess = false, Errors = "Missing refresh token.", StatusCode = 401 });
			}

			var expiredAccessToken = Request.Cookies["accessToken"] ?? string.Empty;
			var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

			var result = await Sender.Send(new RefreshTokenCommand(expiredAccessToken, refreshToken, ipAddress));
			var response = result.ToApiResponse();
			if (response.IsSuccess && response.Data is not null)
				SetAuthCookies(response.Data);

			return StatusCode(response.StatusCode, response);
		}

		[HttpPost("logout")]
		[Authorize]
		// Was entirely missing before - the frontend's Logout button could only clear its
		// local session marker, leaving the httpOnly auth cookie usable until it expired.
		public async Task<IActionResult> Logout(CancellationToken ct)
		{
			var refreshToken = Request.Cookies["refreshToken"];
			if (!string.IsNullOrEmpty(refreshToken))
			{
				var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
				await Sender.Send(new LogoutCommand(refreshToken, ipAddress), ct);
			}

			Response.Cookies.Delete("accessToken");
			Response.Cookies.Delete("refreshToken");

			return StatusCode(200, new ApiResponse<object> { IsSuccess = true, StatusCode = 200 });
		}

		/// <summary>
		/// Sets the access/refresh token httpOnly cookies. Also still returns the tokens in the
		/// JSON response body (see AuthResponseDto) so a frontend on a *different* domain can keep
		/// using the classic "store in localStorage + send as Authorization: Bearer" flow instead -
		/// that flow doesn't depend on cookies/CORS-credentials at all and is simpler to get right
		/// across domains. The two mechanisms are independent; BayTack.Infrastructure's JWT bearer
		/// setup reads the cookie first and falls back to the Authorization header automatically.
		///
		/// SameSite/Secure are picked based on the current request scheme rather than hardcoded:
		/// cross-site cookies are only ever sent by browsers when SameSite=None *and* Secure=true
		/// (HTTPS), so on plain HTTP (local dev) we fall back to SameSite=Lax - the cookie will still
		/// work same-origin, it just won't cross domains until the API is served over HTTPS.
		/// </summary>
		private void SetAuthCookies(AuthResponseDto data)
		{
			var crossSiteCapable = Request.IsHttps;
			var sameSite = crossSiteCapable ? SameSiteMode.None : SameSiteMode.Lax;

			Response.Cookies.Append("accessToken", data.AccessToken, new CookieOptions
			{
				HttpOnly = true,
				Secure = crossSiteCapable,
				SameSite = sameSite,
				Expires = data.AccessTokenExpiration
			});

			Response.Cookies.Append("refreshToken", data.RefreshToken, new CookieOptions
			{
				HttpOnly = true,
				Secure = crossSiteCapable,
				SameSite = sameSite,
				Expires = DateTime.UtcNow.AddDays(7)
			});
		}
	}

	public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
}
