using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Common.DTO;
using BayTack.Application.Features.SavedServices.Commands.AddSavedService;
using BayTack.Application.Features.SavedServices.Commands.RemoveSavedService;
using BayTack.Application.Features.SavedServices.Queries.GetSavedServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Customer
{
    // Backs: Front_end/customer/app/saved  (was mocked -> bt_c_saved, an array of service ids)
    [Authorize]
    [Route("customer/saved")]
    public class SavedController : ApiController
    {
        private readonly ICurrentUserService _currentUser;

        public SavedController(ICurrentUserService currentUser) => _currentUser = currentUser;

        /// <summary>GET /customer/saved -> Service[] (full service objects for saved ids)</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new GetSavedServicesQuery(userId), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>POST /customer/saved/{serviceId} -> { success: true }</summary>
        [HttpPost("{serviceId}")]
        public async Task<IActionResult> Add(string serviceId, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new AddSavedServiceCommand(userId, serviceId), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>DELETE /customer/saved/{serviceId} -> { success: true }</summary>
        [HttpDelete("{serviceId}")]
        public async Task<IActionResult> Remove(string serviceId, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new RemoveSavedServiceCommand(userId, serviceId), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }
    }
}