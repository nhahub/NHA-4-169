using BayTack.API.Extensions;
using BayTack.Application.Features.Users.Command;
using BayTack.Application.Features.Users.Command.DeactivateUser;
using BayTack.Application.Features.Users.Command.DeleteUser;
using BayTack.Application.Features.Users.Command.UpdateUser;
using BayTack.Application.Features.Users.Queries.GetAllUsers;
using BayTack.Application.Features.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Admin
{
	[Authorize]
	public class UsersController : ApiController
	{
		[HttpGet]
		[Authorize(Policy = "Permissions.Users.View")]
		public async Task<IActionResult> GetAll(
			[FromQuery] string? search,
			[FromQuery] string? role,
			[FromQuery] int page = 1,
			[FromQuery] int limit = 20)
		{
			var result = await Sender.Send(new GetAllUsersQuery(search, role, page, limit));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("{id}")]
		[Authorize(Policy = "Permissions.Users.View")] // أو يتم التحقق داخل الـ Handler لو كان المستخدم بيجيب بيانات نفسه
		public async Task<IActionResult> GetById(string id)
		{
			var result = await Sender.Send(new GetUserByIdQuery(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost]
		[Authorize(Policy = "Permissions.Users.Create")]
		public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
		{
			var command = new CreateUserCommand(request.Name, request.Email, request.Phone, request.Role);
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut("{id}")]
		[Authorize(Policy = "Permissions.Users.Update")]
		public async Task<IActionResult> Update(string id, [FromBody] UpdateUserRequest request)
		{
			var updatedBy = CurrentUserId ?? throw new InvalidOperationException("Authenticated user ID is required.");

			var command = new UpdateUserCommand(id, request.Name, request.Email, request.Phone, request.Role, updatedBy);
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPatch("{id}/deactivate")]
		[Authorize(Policy = "Permissions.Users.Deactivate")] // صلاحية تجميد الحسابات
		public async Task<IActionResult> Deactivate(string id)
		{
			var result = await Sender.Send(new DeactivateUserCommand(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpDelete("{id}")]
		[Authorize(Policy = "Permissions.Users.Delete")]
		public async Task<IActionResult> Delete(string id)
		{
			var command = new DeleteUserCommand(id, CurrentUserId, "Deleted via admin API");
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}


}



public sealed record CreateUserRequest(string Name, string Email, string? Phone, string Role);
public sealed record UpdateUserRequest(string Name, string Email, string? Phone, string? Role);
