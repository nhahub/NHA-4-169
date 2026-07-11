using BayTack.Application.Common.DTO;
using BayTack.Application.Features.Providers.Commands.ApproveProvider;
using BayTack.Application.Features.Providers.Commands.ReinstateProvider;
using BayTack.Application.Features.Providers.Commands.SuspendProvider;
using BayTack.Application.Features.Providers.Commands.UpdateProvider;
using BayTack.Application.Features.Providers.Queries.GetAllProviders;
using BayTack.Application.Features.Providers.Queries.GetProviderById;
using BayTack.Application.Features.Providers.Queries.GetProviderStats;
using BayTack.Application.Features.Providers.Queries.GetRecentProviders;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Admin
{
	[Route("api/admin/providers")]
	public class ProvidersController : ApiController
	{
		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] string? categoryId, [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int limit = 20)
		{
			var result = await Sender.Send(new GetAllProvidersQuery(search, categoryId, status, page, limit));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("stats")]
		public async Task<IActionResult> Stats()
		{
			var result = await Sender.Send(new GetProviderStatsQuery());
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("recent")]
		public async Task<IActionResult> Recent([FromQuery] int limit = 5)
		{
			var result = await Sender.Send(new GetRecentProvidersQuery(limit));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(string id)
		{
			var result = await Sender.Send(new GetProviderByIdQuery(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(string id, [FromBody] UpdateProviderRequest body)
		{
			var result = await Sender.Send(new UpdateProviderCommand(id, body.ProviderType, body.YearsOfExperience, body.CategoryId));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPatch("{id}/approve")]
		public async Task<IActionResult> Approve(string id)
		{
			var result = await Sender.Send(new ApproveProviderCommand(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPatch("{id}/suspend")]
		public async Task<IActionResult> Suspend(string id, [FromBody] SuspendProviderRequest body)
		{
			var result = await Sender.Send(new SuspendProviderCommand(id, body.Reason));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPatch("{id}/reinstate")]
		public async Task<IActionResult> Reinstate(string id)
		{
			var result = await Sender.Send(new ReinstateProviderCommand(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}

	public sealed record UpdateProviderRequest(string? ProviderType, int? YearsOfExperience, string? CategoryId);
	public sealed record SuspendProviderRequest(string? Reason);
}
