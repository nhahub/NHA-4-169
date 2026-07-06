using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Admin
{
	// Backs: Front_end/admin/verification.html + verification-review.html
	//        scripts/controllers/verificationController.js / verificationReviewController.js
	// A verification entry is a provider signup pending admin review before it can accept jobs.
	[ApiController]
	[Route("verification")]
	public class VerificationController : ControllerBase
	{
		/// <summary>
		/// GET /verification?status=
		/// -> VerificationEntry[] { id, name, category, providerType, status, submittedAt, img }
		/// status: pending | review
		/// </summary>
		[HttpGet]
		public IActionResult GetQueue([FromQuery] string? status)
			=> StatusCode(501, new { message = "Not implemented: Verification queue" });

		/// <summary>GET /verification/{id} -> VerificationEntry (full detail: docs, ID photos, business license, etc.)</summary>
		[HttpGet("{id}")]
		public IActionResult GetById(string id)
			=> StatusCode(501, new { message = "Not implemented: GetById verification entry" });

		/// <summary>PATCH /verification/{id}/review -> VerificationEntry (status -> review, marks "under review")</summary>
		[HttpPatch("{id}/review")]
		public IActionResult MarkUnderReview(string id)
			=> StatusCode(501, new { message = "Not implemented: Mark under review" });

		/// <summary>PATCH /verification/{id}/approve -> VerificationEntry (status -> approved; mirrors providers/{id}/approve)</summary>
		[HttpPatch("{id}/approve")]
		public IActionResult Approve(string id)
			=> StatusCode(501, new { message = "Not implemented: Approve verification" });

		/// <summary>PATCH /verification/{id}/reject  Body: { reason } -> VerificationEntry (status -> suspended)</summary>
		[HttpPatch("{id}/reject")]
		public IActionResult Reject(string id, [FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Reject verification" });
	}
}
