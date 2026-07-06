using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Provider
{
	// Consumed by: Front_end/provider/app/bid/scripts/services/bidService.js
	[ApiController]
	public class BidsController : ControllerBase
	{
		/// <summary>
		/// POST /bids
		/// Body:     { jobId, price, durationDays, availability, notes }
		/// Response: { success: true, bidId }
		/// </summary>
		[HttpPost("bids")]
		public IActionResult Submit([FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Submit bid" });

		/// <summary>DELETE /bids/{bidId} -> { success: true } (retract a submitted bid)</summary>
		[HttpDelete("bids/{bidId}")]
		public IActionResult Retract(string bidId)
			=> StatusCode(501, new { message = "Not implemented: Retract bid" });

		/// <summary>GET /jobs/{jobId}/bids -> Bid[] (all bids placed on a job, e.g. for the customer to compare)</summary>
		[HttpGet("jobs/{jobId}/bids")]
		public IActionResult GetBidsForJob(string jobId)
			=> StatusCode(501, new { message = "Not implemented: Get bids for job" });
	}
}
