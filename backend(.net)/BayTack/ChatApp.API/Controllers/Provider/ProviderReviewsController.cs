using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Provider
{
	// Consumed by: Front_end/provider/app/reviews/scripts/services/reviewService.js
	[ApiController]
	[Route("provider/reviews")]
	public class ProviderReviewsController : ControllerBase
	{
		/// <summary>
		/// GET /provider/reviews?filter=&page=&limit=
		/// -> { reviews: Review[] { id, author, avatarUrl, rating, text, serviceName, orderId, serviceIcon, createdAt, verified }, total }
		/// </summary>
		[HttpGet]
		public IActionResult GetReviews([FromQuery] string filter = "all", [FromQuery] int page = 1, [FromQuery] int limit = 10)
			=> StatusCode(501, new { message = "Not implemented: Get provider reviews" });

		/// <summary>GET /provider/reviews/stats -> { averageRating, totalReviews, satisfactionPct, fiveStarCount, weeklyCount }</summary>
		[HttpGet("stats")]
		public IActionResult GetStats()
			=> StatusCode(501, new { message = "Not implemented: Get review stats" });

		/// <summary>GET /provider/reviews/export?filter= -> text/csv file download</summary>
		[HttpGet("export")]
		public IActionResult ExportCsv([FromQuery] string filter = "all")
			=> StatusCode(501, new { message = "Not implemented: Export reviews CSV" });

		/// <summary>POST /provider/reviews/{id}/respond  Body: { text } -> Review (with providerResponse set)</summary>
		[HttpPost("{id}/respond")]
		public IActionResult Respond(string id, [FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Respond to review" });
	}
}
