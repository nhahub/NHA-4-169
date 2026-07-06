using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Customer
{
	// Backs: Front_end/customer/app/saved  (currently mocked -> bt_c_saved, an array of service ids)
	[ApiController]
	[Route("customer/saved")]
	public class SavedController : ControllerBase
	{
		/// <summary>GET /customer/saved -> Service[] (full service objects for saved ids)</summary>
		[HttpGet]
		public IActionResult GetAll()
			=> StatusCode(501, new { message = "Not implemented: GetAll saved services" });

		/// <summary>POST /customer/saved/{serviceId} -> { success: true }</summary>
		[HttpPost("{serviceId}")]
		public IActionResult Add(string serviceId)
			=> StatusCode(501, new { message = "Not implemented: Save service" });

		/// <summary>DELETE /customer/saved/{serviceId} -> { success: true }</summary>
		[HttpDelete("{serviceId}")]
		public IActionResult Remove(string serviceId)
			=> StatusCode(501, new { message = "Not implemented: Unsave service" });
	}
}
