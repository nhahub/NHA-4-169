using BayTack.API.Extensions;
using BayTack.Application.Features.Users.Command;
using BayTack.Application.Features.Users.Command.UpdateUser;
using BayTack.Application.Features.Users.Queries.GetAllUsers;
using BayTack.Application.Features.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Admin
{
	public class UsersController : ApiController
	{
		[HttpGet]
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

		/// <summary>GET /users/{id} -> User</summary>
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(string id)
		{
			var result = await Sender.Send(new GetUserByIdQuery(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		/// <summary>POST /users  Body: { name, email, phone, role } -> User</summary>
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
		{
			var command = new CreateUserCommand(request.Name, request.Email, request.Phone, request.Role);
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}


		/// <summary>PUT /users/{id}  Body: { name, email, phone, role } -> User</summary>
		[HttpPut("{id}")]
		public async Task<IActionResult> Update(string id, [FromBody] UpdateUserRequest request)
		{
			// UpdatedBy comes from the authenticated admin (claims), never from the request body.
			var updatedBy = CurrentUserId ?? throw new InvalidOperationException("Authenticated user ID is required.");

			var command = new UpdateUserCommand(id, request.Name, request.Email, request.Phone, request.Role, updatedBy);
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}





	}
}


public sealed record CreateUserRequest(string Name, string Email, string? Phone, string Role);
public sealed record UpdateUserRequest(string Name, string Email, string? Phone, string? Role);








//	/// <summary>PATCH /users/{id}/deactivate -> { success: true }</summary>
//	[HttpPatch("{id:int}/deactivate")]
//	public async Task<IActionResult> Deactivate(int id)
//	{
//		var result = await Sender.Send(new DeactivateUserCommand(id));
//		var response = result.ToApiResponse();
//		return StatusCode(response.StatusCode, response);
//	}

//	/// <summary>DELETE /users/{id} -> { success: true } (soft delete)</summary>
//	[HttpDelete("{id:int}")]
//	public async Task<IActionResult> Delete(int id)
//	{
//		var command = new DeleteUserCommand(id, _currentUser.UserId, "Deleted via admin API");
//		var result = await Sender.Send(command);
//		var response = result.ToApiResponse();
//		return StatusCode(response.StatusCode, response);
//	}





