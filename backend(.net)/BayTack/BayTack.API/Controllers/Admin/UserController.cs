using BayTack.Application.Features.Users.Queries.GetAllUsers;
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
	}
}
