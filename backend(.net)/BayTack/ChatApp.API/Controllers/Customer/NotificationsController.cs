using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Customer
{
	// Backs: Front_end/customer/app/notifications  (currently mocked -> bt_c_notifications)
	[ApiController]
	[Route("customer/notifications")]
	public class NotificationsController : ControllerBase
	{
		/// <summary>GET /customer/notifications -> Notification[] { id, group, read, icon, title, body, createdAt }</summary>
		[HttpGet]
		public IActionResult GetAll()
			=> StatusCode(501, new { message = "Not implemented: GetAll notifications" });

		/// <summary>PATCH /customer/notifications/{id}/read -> Notification (read -> true)</summary>
		[HttpPatch("{id}/read")]
		public IActionResult MarkRead(string id)
			=> StatusCode(501, new { message = "Not implemented: Mark notification read" });

		/// <summary>PATCH /customer/notifications/read-all -> { success: true }</summary>
		[HttpPatch("read-all")]
		public IActionResult MarkAllRead()
			=> StatusCode(501, new { message = "Not implemented: Mark all notifications read" });
	}
}
