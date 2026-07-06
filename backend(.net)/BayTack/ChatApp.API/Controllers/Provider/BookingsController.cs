using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Provider
{
	// Consumed by: Front_end/provider/app/bookings/scripts/services/bookingService.js
	[ApiController]
	[Route("bookings")]
	public class BookingsController : ControllerBase
	{
		/// <summary>GET /bookings?status=&range= -> Booking[] { id, customer, service, scheduledAt, status, address }</summary>
		[HttpGet]
		public IActionResult GetAll([FromQuery] string? status, [FromQuery] string? range)
			=> StatusCode(501, new { message = "Not implemented: GetAll bookings" });

		/// <summary>GET /bookings/{id} -> Booking (full detail)</summary>
		[HttpGet("{id}")]
		public IActionResult GetById(string id)
			=> StatusCode(501, new { message = "Not implemented: GetById booking" });

		/// <summary>PATCH /bookings/{id}/accept -> Booking (status -> confirmed)</summary>
		[HttpPatch("{id}/accept")]
		public IActionResult Accept(string id)
			=> StatusCode(501, new { message = "Not implemented: Accept booking" });

		/// <summary>PATCH /bookings/{id}/decline -> Booking (status -> declined)</summary>
		[HttpPatch("{id}/decline")]
		public IActionResult Decline(string id)
			=> StatusCode(501, new { message = "Not implemented: Decline booking" });

		/// <summary>PATCH /bookings/{id}/complete -> Booking (status -> completed)</summary>
		[HttpPatch("{id}/complete")]
		public IActionResult Complete(string id)
			=> StatusCode(501, new { message = "Not implemented: Complete booking" });

		/// <summary>PATCH /bookings/{id}  Body: { status } -> Booking (generic status update)</summary>
		[HttpPatch("{id}")]
		public IActionResult UpdateStatus(string id, [FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Update booking status" });

		/// <summary>POST /bookings/manual  Body: { customerName, service, scheduledAt, address, ... } -> Booking
		/// (provider manually logging a job booked off-platform)</summary>
		[HttpPost("manual")]
		public IActionResult CreateManual([FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Create manual booking" });
	}
}
