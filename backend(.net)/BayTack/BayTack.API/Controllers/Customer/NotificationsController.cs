using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Common.DTO;
using BayTack.Application.Features.Notifications.Commands.MarkAllNotificationsRead;
using BayTack.Application.Features.Notifications.Commands.MarkNotificationRead;
using BayTack.Application.Features.Notifications.Queries.GetMyNotifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Customer
{
	// Backs: Front_end/customer/app/notifications  (was mocked -> bt_c_notifications)
	[Authorize]
	[Route("customer/notifications")]
	public class NotificationsController : ApiController
	{
		private readonly ICurrentUserService _currentUser;

		public NotificationsController(ICurrentUserService currentUser) => _currentUser = currentUser;

		[HttpGet]
		[Authorize(Policy = "Permissions.Notifications.CustomerView")]
		public async Task<IActionResult> GetAll(CancellationToken ct)
		{
			var userId = _currentUser.UserId;
			if (userId is null) return Unauthorized();

			var result = await Sender.Send(new GetMyNotificationsQuery(userId), ct);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPatch("{id}/read")]
		[Authorize(Policy = "Permissions.Notifications.CustomerUpdate")]
		public async Task<IActionResult> MarkRead(string id, CancellationToken ct)
		{
			var userId = _currentUser.UserId;
			if (userId is null) return Unauthorized();

			var result = await Sender.Send(new MarkNotificationReadCommand(userId, id), ct);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPatch("read-all")]
		[Authorize(Policy = "Permissions.Notifications.CustomerUpdate")]
		public async Task<IActionResult> MarkAllRead(CancellationToken ct)
		{
			var userId = _currentUser.UserId;
			if (userId is null) return Unauthorized();

			var result = await Sender.Send(new MarkAllNotificationsReadCommand(userId), ct);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}
}