using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Customer
{
	// Backs: Front_end/customer/app/post-request, requests/  (currently mocked -> bt_c_requests)
	// A "request" is a customer job post that providers submit offers/bids against.
	[ApiController]
	[Route("customer/requests")]
	public class RequestsController : ControllerBase
	{
		/// <summary>
		/// GET /customer/requests
		/// -> Request[] { id, title, category, budget, deadline, location, status, offers: Offer[] }
		/// status: open | assigned | closed
		/// </summary>
		[HttpGet]
		public IActionResult GetAll()
			=> StatusCode(501, new { message = "Not implemented: GetAll customer requests" });

		/// <summary>GET /customer/requests/{id} -> Request (with offers from providers)</summary>
		[HttpGet("{id}")]
		public IActionResult GetById(string id)
			=> StatusCode(501, new { message = "Not implemented: GetById request" });

		/// <summary>POST /customer/requests  Body: { title, category, budget, deadline, location } -> Request</summary>
		[HttpPost]
		public IActionResult Create([FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Create request" });

		/// <summary>DELETE /customer/requests/{id} -> { success: true }</summary>
		[HttpDelete("{id}")]
		public IActionResult Delete(string id)
			=> StatusCode(501, new { message = "Not implemented: Delete request" });

		/// <summary>
		/// POST /customer/requests/{id}/offers/{offerId}/accept -> Request (status -> assigned, becomes an Order)
		/// </summary>
		[HttpPost("{id}/offers/{offerId}/accept")]
		public IActionResult AcceptOffer(string id, string offerId)
			=> StatusCode(501, new { message = "Not implemented: Accept offer on request" });
	}
}
