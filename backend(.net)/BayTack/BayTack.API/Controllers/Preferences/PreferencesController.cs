using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Common.DTO;
using BayTack.Application.Features.Preferences.Commands.UpdateMyPreferences;
using BayTack.Application.Features.Preferences.Queries.GetMyPreferences;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Preferences
{
    /// <summary>Request body for PATCH /preferences.</summary>
    public sealed record UpdatePreferencesRequest(
        bool ShowActiveJobWidget,
        bool NotifyNewOffers,
        bool NotifyOrderUpdates,
        bool NotifyPromotions,
        bool NotifySms,
        bool ShowProfileToMessagedProviders,
        bool ShareLocationWithActiveOrders,
        string Language,
        string Theme);

    // Shared across Customer/Provider/Admin - backs the toggle/language/appearance UI
    // on Front_end/customer/app/settings (built in a previous session, left disabled
    // pending this endpoint) and can back the equivalent Provider/Admin settings pages
    // once those exist, since the fields don't differ by role.
    [Authorize]
    [Route("api/v{version:apiVersion}/preferences")]
    public class PreferencesController : ApiController
    {
		public PreferencesController(ISender sender, ICurrentUserService currentUser)
								: base(sender, currentUser)
		{
		}

		[HttpGet]
        [Authorize(Policy = "Permissions.Auth.General")]
        public async Task<IActionResult> GetMyPreferences(CancellationToken ct)
        {
            var userId = CurrentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new GetMyPreferencesQuery(userId), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch]
        [Authorize(Policy = "Permissions.Auth.General")]
        public async Task<IActionResult> UpdateMyPreferences(UpdatePreferencesRequest request, CancellationToken ct)
        {
            var userId = CurrentUser.UserId;
            if (userId is null) return Unauthorized();

            var command = new UpdateMyPreferencesCommand(
                userId,
                request.ShowActiveJobWidget,
                request.NotifyNewOffers,
                request.NotifyOrderUpdates,
                request.NotifyPromotions,
                request.NotifySms,
                request.ShowProfileToMessagedProviders,
                request.ShareLocationWithActiveOrders,
                request.Language,
                request.Theme);

            var result = await Sender.Send(command, ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }
    }
}
