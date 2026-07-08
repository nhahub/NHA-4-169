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

        /// <summary>GET /customer/notifications -> Notification[] { id, group, read, icon, title, body, createdAt }</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new GetMyNotificationsQuery(userId), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>PATCH /customer/notifications/{id}/read -> Notification (read -> true)</summary>
        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkRead(string id, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new MarkNotificationReadCommand(userId, id), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>PATCH /customer/notifications/read-all -> { success: true }</summary>
        [HttpPatch("read-all")]
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