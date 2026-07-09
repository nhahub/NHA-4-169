using BayTack.Application.Common.DTO;
using BayTack.Application.Features.Roles.Commands.CreateRole;
using BayTack.Application.Features.Roles.Commands.DeleteRole;
using BayTack.Application.Features.Roles.Commands.SetRolePermissions;
using BayTack.Application.Features.Roles.Commands.UpdateRole;
using BayTack.Application.Features.Roles.Queries.GetAllRoles;
using BayTack.Application.Features.Roles.Queries.GetPermissions;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Admin
{
	public class RolesController : ApiController
	{
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await Sender.Send(new GetAllRolesQuery());
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateRoleCommand command)
		{
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(string id, [FromBody] UpdateRoleRequest body)
		{
			var result = await Sender.Send(new UpdateRoleCommand(id, body.Name));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await Sender.Send(new DeleteRoleCommand(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost("{id}/permissions")]
		public async Task<IActionResult> SetPermissions(string id, [FromBody] SetPermissionsRequest body)
		{
			var result = await Sender.Send(new SetRolePermissionsCommand(id, body.PermissionIds ?? new()));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("/api/Permissions")]
		public async Task<IActionResult> GetPermissions()
		{
			var result = await Sender.Send(new GetPermissionsQuery());
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}

	public sealed record UpdateRoleRequest(string Name);
	public sealed record SetPermissionsRequest(List<string>? PermissionIds);
}
