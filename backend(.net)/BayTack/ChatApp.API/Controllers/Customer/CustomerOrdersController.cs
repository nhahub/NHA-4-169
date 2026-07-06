using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Customer
{
	// Backs: Front_end/customer/app/orders, dashboard  (currently mocked -> bt_c_orders)
	[ApiController]
	[Route("customer/orders")]
	public class CustomerOrdersController : ControllerBase
	{
		/// <summary>
		/// GET /customer/orders?status=
		/// -> Order[] { id, serviceId, title, provider, avatar, price, status, progress }
		/// status: active | completed | cancelled
		/// </summary>
		[HttpGet]
		public IActionResult GetAll([FromQuery] string? status)
			=> StatusCode(501, new { message = "Not implemented: GetAll customer orders" });

		/// <summary>GET /customer/orders/{id} -> Order (full detail)</summary>
		[HttpGet("{id}")]
		public IActionResult GetById(string id)
			=> StatusCode(501, new { message = "Not implemented: GetById customer order" });

		/// <summary>POST /customer/orders  Body: { serviceId, tier } -> Order (booking a catalog service directly)</summary>
		[HttpPost]
		public IActionResult Create([FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Create customer order" });

		/// <summary>PATCH /customer/orders/{id}/cancel -> Order (status -> cancelled)</summary>
		[HttpPatch("{id}/cancel")]
		public IActionResult Cancel(string id)
			=> StatusCode(501, new { message = "Not implemented: Cancel customer order" });
	}
}
