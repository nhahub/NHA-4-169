using BayTack.Application.Common.DTO;
using BayTack.Application.Features.Settings.Commands.UpdateSettings;
using BayTack.Application.Features.Settings.Queries.GetSettings;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Admin
{
	public class SettingsController : ApiController
	{
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var result = await Sender.Send(new GetSettingsQuery());
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] UpdateSettingsCommand command)
		{
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}
}
