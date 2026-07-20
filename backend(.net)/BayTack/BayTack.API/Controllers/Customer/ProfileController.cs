using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Common.DTO;
using BayTack.Application.Features.Users.Command.DeleteUser;
using BayTack.Application.Features.Users.Command.UpdateUser;
using BayTack.Application.Features.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Customer
{
    /// <summary>Request body for PATCH /customer/profile.</summary>
    public sealed record UpdateMyProfileRequest(string FullName, string? Phone);

    // Backs: Front_end/customer/app/profile - previously had no backend endpoint at all,
    // so Save Changes / Delete Account only ever touched localStorage. Reuses the existing
    // admin UpdateUser/DeleteUser commands (same underlying user record) rather than
    // duplicating that logic, scoped to the caller's own id and without a Role parameter
    // so a customer can't grant themselves a different role.
    [Authorize]
    [Route("customer/profile")]
    public class ProfileController : ApiController
    {
        private readonly ICurrentUserService _currentUser;

        public ProfileController(ICurrentUserService currentUser) => _currentUser = currentUser;

        [HttpGet]
        [Authorize(Policy = "Permissions.Auth.General")]
        public async Task<IActionResult> GetMyProfile(CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new GetUserByIdQuery(userId), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch]
        [Authorize(Policy = "Permissions.Auth.General")]
        public async Task<IActionResult> UpdateMyProfile(UpdateMyProfileRequest request, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            var email = _currentUser.Email;
            if (userId is null || email is null) return Unauthorized();

            // Role intentionally left null - UpdateUserCommandHandler/UpdateUserAsync only
            // changes the role when a non-empty value is passed, so this can never be used
            // to self-promote (see IdentityService.UpdateUserAsync).
            var command = new UpdateUserCommand(userId, request.FullName, email, request.Phone, null, userId);
            var result = await Sender.Send(command, ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        [Authorize(Policy = "Permissions.Auth.General")]
        public async Task<IActionResult> DeleteMyAccount(CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new DeleteUserCommand(userId, userId, "Deleted by customer via profile settings."), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }
    }
}
