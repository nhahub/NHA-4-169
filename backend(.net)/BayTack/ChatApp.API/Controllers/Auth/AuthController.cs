using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Auth
{
	// Consumed by:
	//  - Front_end/scripts/services/authService.js                (admin)
	//  - Front_end/provider/app/bid/scripts/services/authService.js
	//  - Front_end/provider/app/bookings/scripts/services/authService.js
	//  - Front_end/customer (landing.js login, session stored as ek_user_session)
	[ApiController]
	[Route("auth")]
	public class AuthController : ControllerBase
	{
		/// <summary>
		/// POST /auth/login
		/// Body:     { "email": string, "password": string }
		/// Response: { "accessToken": string, "refreshToken": string, "user": { id, name, email, role } }
		/// </summary>
		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginRequest request)
		{
			// TODO: validate credentials, issue JWT access/refresh tokens
			return StatusCode(501, new { message = "Not implemented: Login" });
		}

		/// <summary>
		/// POST /auth/register
		/// Body:     { "name": string, "email": string, "phone": string, "password": string, "role": "customer" | "provider" }
		/// Response: { "accessToken": string, "refreshToken": string, "user": { ... } }
		/// </summary>
		[HttpPost("register")]
		public IActionResult Register([FromBody] RegisterRequest request)
		{
			// TODO: create account, hash password, issue tokens
			return StatusCode(501, new { message = "Not implemented: Register" });
		}

		/// <summary>
		/// POST /auth/logout
		/// Response: { "success": true }
		/// </summary>
		[HttpPost("logout")]
		public IActionResult Logout()
		{
			// TODO: revoke refresh token / clear session
			return StatusCode(501, new { message = "Not implemented: Logout" });
		}
	}

	public record LoginRequest(string Email, string Password);

	public record RegisterRequest(string Name, string Email, string Phone, string Password, string Role);
}
