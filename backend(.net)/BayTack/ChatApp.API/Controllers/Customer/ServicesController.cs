using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Customer
{
	// Backs: Front_end/customer/app/browse, service/, saved/  (currently mocked in customer-data.js -> bt_c_services)
	[ApiController]
	[Route("services")]
	public class ServicesController : ControllerBase
	{
		/// <summary>
		/// GET /services?category=&search=
		/// -> Service[] { id, title, category, icon, provider, providerId, avatar, rating, tiers: { basic, standard, premium } }
		/// </summary>
		[HttpGet]
		public IActionResult GetAll([FromQuery] string? category, [FromQuery] string? search)
			=> StatusCode(501, new { message = "Not implemented: GetAll services" });

		/// <summary>GET /services/{id} -> Service (full detail with pricing tiers)</summary>
		[HttpGet("{id}")]
		public IActionResult GetById(string id)
			=> StatusCode(501, new { message = "Not implemented: GetById service" });
	}
}
