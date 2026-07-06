using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Admin
{
	// Consumed by: Front_end/scripts/services/rolesService.js (Admin > Roles & Permissions page)
	[ApiController]
	public class RolesController : ControllerBase
	{
		/// <summary>GET /roles -> Role[] { id, name, permissions[], userCount }</summary>
		[HttpGet("roles")]
		public IActionResult GetAll()
			=> StatusCode(501, new { message = "Not implemented: GetAll roles" });

		/// <summary>GET /roles/{id} -> Role</summary>
		[HttpGet("roles/{id}")]
		public IActionResult GetById(int id)
			=> StatusCode(501, new { message = "Not implemented: GetById role" });

		/// <summary>POST /roles  Body: { name, permissions[] } -> Role</summary>
		[HttpPost("roles")]
		public IActionResult Create([FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Create role" });

		/// <summary>PUT /roles/{id}  Body: { name, permissions[] } -> Role</summary>
		[HttpPut("roles/{id}")]
		public IActionResult Update(int id, [FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Update role" });

		/// <summary>DELETE /roles/{id} -> { success: true }</summary>
		[HttpDelete("roles/{id}")]
		public IActionResult Delete(int id)
			=> StatusCode(501, new { message = "Not implemented: Delete role" });

		/// <summary>POST /roles/{roleId}/permissions  Body: { permissionIds: string[] } -> Role</summary>
		[HttpPost("roles/{roleId}/permissions")]
		public IActionResult SetPermissions(int roleId, [FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Set role permissions" });

		/// <summary>GET /permissions -> Permission[] { id, label, group }</summary>
		[HttpGet("permissions")]
		public IActionResult GetPermissions()
			=> StatusCode(501, new { message = "Not implemented: GetPermissions" });
	}
}
