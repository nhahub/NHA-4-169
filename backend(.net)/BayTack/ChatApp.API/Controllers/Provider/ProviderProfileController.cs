using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Provider
{
	// Consumed by: Front_end/provider/app/settings/scripts/services/providerService.js
	[ApiController]
	[Route("provider")]
	public class ProviderProfileController : ControllerBase
	{
		/// <summary>GET /provider/profile -> { name, email, phone, bio, categories[], avatarUrl, ... } (current signed-in provider)</summary>
		[HttpGet("profile")]
		public IActionResult GetProfile()
			=> StatusCode(501, new { message = "Not implemented: Get provider profile" });

		/// <summary>PUT /provider/profile  Body: { name, email, phone, bio, categories[] } -> updated profile</summary>
		[HttpPut("profile")]
		public IActionResult UpdateProfile([FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Update provider profile" });

		/// <summary>GET /provider/schedule -> { workingDays[], workingHours: { start, end }, exceptions[] }</summary>
		[HttpGet("schedule")]
		public IActionResult GetSchedule()
			=> StatusCode(501, new { message = "Not implemented: Get provider schedule" });

		/// <summary>PUT /provider/schedule  Body: { schedule } -> updated schedule</summary>
		[HttpPut("schedule")]
		public IActionResult UpdateSchedule([FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Update provider schedule" });

		/// <summary>GET /provider/preferences -> { notifyEmail, notifySms, autoAcceptJobs, serviceRadiusKm, ... }</summary>
		[HttpGet("preferences")]
		public IActionResult GetPreferences()
			=> StatusCode(501, new { message = "Not implemented: Get provider preferences" });

		/// <summary>PATCH /provider/preferences  Body: partial preferences -> updated preferences</summary>
		[HttpPatch("preferences")]
		public IActionResult UpdatePreferences([FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Update provider preferences" });

		/// <summary>POST /provider/pause -> { success: true } (temporarily stop receiving new jobs)</summary>
		[HttpPost("pause")]
		public IActionResult Pause()
			=> StatusCode(501, new { message = "Not implemented: Pause provider account" });
	}
}
