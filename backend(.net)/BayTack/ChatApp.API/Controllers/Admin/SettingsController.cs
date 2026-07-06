using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Admin
{
	// Consumed by: Front_end/scripts/services/settingsService.js and scripts/controllers/settingsController.js
	[ApiController]
	[Route("settings")]
	public class SettingsController : ControllerBase
	{
		/// <summary>GET /settings -> { siteName, supportEmail, currency, maintenanceMode, commissionRate, notificationsEnabled }</summary>
		[HttpGet]
		public IActionResult Get()
			=> StatusCode(501, new { message = "Not implemented: Get settings" });

		/// <summary>PUT /settings  Body: { siteName, supportEmail, currency, maintenanceMode, commissionRate, notificationsEnabled } -> same shape</summary>
		[HttpPut]
		public IActionResult Update([FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Update settings" });
	}
}
