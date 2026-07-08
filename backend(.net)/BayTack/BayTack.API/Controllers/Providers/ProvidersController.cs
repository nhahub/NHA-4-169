using BayTack.API.Extensions;
using BayTack.Application.Features.Providers.Commands.CreateProviderProfile;
using BayTack.Application.Features.Providers.Commands.UpdateProviderBio;
using BayTack.Application.Features.Providers.Queries.GetProviderProfileById;
using BayTack.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Providers
{
	public class ProvidersController : ApiController
	{
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateProviderProfileRequest request)
		{
			var command = new CreateProviderProfileCommand(
				request.UserId,
				request.ProviderType,
				request.YearsOfExperience,
				request.Bio);

			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(string id)
		{
			var result = await Sender.Send(new GetProviderProfileByIdQuery(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPatch("{id}/bio")]
		public async Task<IActionResult> UpdateBio(string id, [FromBody] UpdateProviderBioRequest request)
		{
			var command = new UpdateProviderBioCommand(id, request.Bio, request.UpdatedBy);
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}

	public sealed record CreateProviderProfileRequest(
		string UserId,
		ProviderType ProviderType,
		int YearsOfExperience,
		string? Bio);

	public sealed record UpdateProviderBioRequest(string Bio, string UpdatedBy);
}
