using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Admin
{
	// Backs: Front_end/admin/orders.html + scripts/controllers/ordersController.js
	// NOTE: the current front-end renders static demo rows and paginates them client-side.
	// These endpoints are the real-backend replacement the team should wire the page up to.
	[ApiController]
	[Route("orders")]
	public class OrdersController : ControllerBase
	{
		/// <summary>
		/// GET /orders?search=&status=&range=&page=&limit=
		/// -> { items: Order[] { id, customer, provider, service, amount, status, createdAt }, total }
		/// status: pending | in-progress | completed | cancelled ; range: today | week | month | all
		/// </summary>
		[HttpGet]
		public IActionResult GetAll([FromQuery] string? search, [FromQuery] string? status, [FromQuery] string? range, [FromQuery] int page = 1, [FromQuery] int limit = 10)
			=> StatusCode(501, new { message = "Not implemented: GetAll orders" });

		/// <summary>GET /orders/{id} -> Order (full detail incl. payment info)</summary>
		[HttpGet("{id}")]
		public IActionResult GetById(string id)
			=> StatusCode(501, new { message = "Not implemented: GetById order" });

		/// <summary>PATCH /orders/{id}/status  Body: { status } -> Order</summary>
		[HttpPatch("{id}/status")]
		public IActionResult UpdateStatus(string id, [FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Update order status" });
	}
}
