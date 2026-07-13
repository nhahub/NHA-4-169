using BayTack.API.Extensions;
using BayTack.Application.Features.Orders.Commands.AcceptBooking;
using BayTack.Application.Features.Orders.Commands.CompleteBooking;
using BayTack.Application.Features.Orders.Commands.DeclineBooking;
using BayTack.Application.Features.Orders.Commands.UpdateBookingStatus;
using BayTack.Application.Features.Orders.Queries.GetAllBookings;
using BayTack.Application.Features.Orders.Queries.GetBookingById;
using BayTack.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers
{
	[Authorize]
	public class BookingsController : ApiController
	{
		[HttpGet]
		[Authorize(Policy = "Permissions.Bookings.View")] // مسموح للطرفين لرؤية قائمة الحجوزات الخاصة بهم
		public async Task<IActionResult> GetAll([FromQuery] string providerId, [FromQuery] string? status)
		{
			var result = await Sender.Send(new GetAllBookingsQuery(providerId, status));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("{id}")]
		[Authorize(Policy = "Permissions.Bookings.View")] // مسموح للطرفين لرؤية تفاصيل الحجز
		public async Task<IActionResult> GetById(string id)
		{
			var result = await Sender.Send(new GetBookingByIdQuery(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPatch("{id}/accept")]
		[Authorize(Policy = "Permissions.Bookings.Manage")] // صلاحية اتخاذ إجراء قبول الحجز
		public async Task<IActionResult> Accept(string id, [FromBody] ChangedByRequest request)
		{
			var result = await Sender.Send(new AcceptBookingCommand(id, request.ChangedBy));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPatch("{id}/decline")]
		[Authorize(Policy = "Permissions.Bookings.Manage")] // صلاحية اتخاذ إجراء رفض الحجز
		public async Task<IActionResult> Decline(string id, [FromBody] ChangedByRequest request)
		{
			var result = await Sender.Send(new DeclineBookingCommand(id, request.ChangedBy));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPatch("{id}/complete")]
		[Authorize(Policy = "Permissions.Bookings.Manage")] // صلاحية إنهاء وإتمام الحجز
		public async Task<IActionResult> Complete(string id, [FromBody] ChangedByRequest request)
		{
			var result = await Sender.Send(new CompleteBookingCommand(id, request.ChangedBy));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPatch("{id}")]
		[Authorize(Policy = "Permissions.Bookings.Manage")] // تغيير الحالة العام
		public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateBookingStatusRequest request)
		{
			var command = new UpdateBookingStatusCommand(id, request.Status, request.ChangedBy);
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}
	public sealed record ChangedByRequest(string ChangedBy);

	public sealed record UpdateBookingStatusRequest(OrderStatus Status, string ChangedBy);
}
