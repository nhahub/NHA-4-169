using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Admin
{
	// Consumed by: Front_end/scripts/services/usersService.js (Admin > User Management page)
	[ApiController]
	[Route("users")]
	public class UsersController : ControllerBase
	{
		/// <summary>GET /users?search=&role=&page=&limit= -> { items: User[], total }</summary>
		[HttpGet]
		public IActionResult GetAll([FromQuery] string? search, [FromQuery] string? role, [FromQuery] int page = 1, [FromQuery] int limit = 20)
			=> StatusCode(501, new { message = "Not implemented: GetAll users" });

		/// <summary>GET /users/{id} -> User</summary>
		[HttpGet("{id}")]
		public IActionResult GetById(string id)
			=> StatusCode(501, new { message = "Not implemented: GetById user" });

		/// <summary>POST /users  Body: { name, email, phone, role } -> User</summary>
		[HttpPost]
		public IActionResult Create([FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Create user" });

		/// <summary>PUT /users/{id}  Body: { name, email, phone, role } -> User</summary>
		[HttpPut("{id}")]
		public IActionResult Update(string id, [FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Update user" });

		/// <summary>PATCH /users/{id}/deactivate -> { success: true }</summary>
		[HttpPatch("{id}/deactivate")]
		public IActionResult Deactivate(string id)
			=> StatusCode(501, new { message = "Not implemented: Deactivate user" });

		/// <summary>DELETE /users/{id} -> { success: true }</summary>
		[HttpDelete("{id}")]
		public IActionResult Delete(string id)
			=> StatusCode(501, new { message = "Not implemented: Delete user" });
	}
}
