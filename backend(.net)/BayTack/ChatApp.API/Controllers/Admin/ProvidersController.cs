using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Admin
{
	// Consumed by: Front_end/scripts/services/providersService.js (Admin > Providers page)
	// GET /providers is also reused by the customer/provider browse pages (public subset of fields).
	[ApiController]
	[Route("providers")]
	public class ProvidersController : ControllerBase
	{
		/// <summary>GET /providers?search=&category=&status=&page=&limit= -> { items: Provider[], total }</summary>
		[HttpGet]
		public IActionResult GetAll([FromQuery] string? search, [FromQuery] string? category, [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int limit = 20)
			=> StatusCode(501, new { message = "Not implemented: GetAll providers" });

		/// <summary>GET /providers/stats -> { total, pending, approved, suspended } (call before {id} route to avoid clash)</summary>
		[HttpGet("stats")]
		public IActionResult Stats()
			=> StatusCode(501, new { message = "Not implemented: Provider stats" });

		/// <summary>GET /providers/recent?limit= -> Provider[] (latest signups, used on Analytics page)</summary>
		[HttpGet("recent")]
		public IActionResult Recent([FromQuery] int limit = 5)
			=> StatusCode(501, new { message = "Not implemented: Recent providers" });

		/// <summary>GET /providers/{id} -> Provider (full profile incl. verification docs)</summary>
		[HttpGet("{id}")]
		public IActionResult GetById(string id)
			=> StatusCode(501, new { message = "Not implemented: GetById provider" });

		/// <summary>POST /providers  Body: { name, category, providerType, ... } -> Provider</summary>
		[HttpPost]
		public IActionResult Create([FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Create provider" });

		/// <summary>PUT /providers/{id}  Body: { ...fields } -> Provider</summary>
		[HttpPut("{id}")]
		public IActionResult Update(string id, [FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Update provider" });

		/// <summary>PATCH /providers/{id}/approve -> Provider (status -> approved)</summary>
		[HttpPatch("{id}/approve")]
		public IActionResult Approve(string id)
			=> StatusCode(501, new { message = "Not implemented: Approve provider" });

		/// <summary>PATCH /providers/{id}/suspend  Body: { reason } -> Provider (status -> suspended)</summary>
		[HttpPatch("{id}/suspend")]
		public IActionResult Suspend(string id, [FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Suspend provider" });

		/// <summary>PATCH /providers/{id}/reinstate -> Provider (status -> approved again)</summary>
		[HttpPatch("{id}/reinstate")]
		public IActionResult Reinstate(string id)
			=> StatusCode(501, new { message = "Not implemented: Reinstate provider" });

		/// <summary>DELETE /providers/{id} -> { success: true }</summary>
		[HttpDelete("{id}")]
		public IActionResult Delete(string id)
			=> StatusCode(501, new { message = "Not implemented: Delete provider" });
	}
}
