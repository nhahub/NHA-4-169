using System.Text;
using BayTack.API.Extensions;
using BayTack.Application.Features.Providers.Commands.RespondToReview;
using BayTack.Application.Features.Providers.Queries.ExportProviderReviewsCsv;
using BayTack.Application.Features.Providers.Queries.GetProviderReviews;
using BayTack.Application.Features.Providers.Queries.GetProviderReviewStats;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers
{
	public class ProviderReviewsController : ApiController
	{
		[HttpGet]
		public async Task<IActionResult> GetReviews(
			[FromQuery] string providerId, [FromQuery] string? filter, [FromQuery] int page = 1, [FromQuery] int limit = 10)
		{
			var result = await Sender.Send(new GetProviderReviewsQuery(providerId, filter, page, limit));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("stats")]
		public async Task<IActionResult> GetStats([FromQuery] string providerId)
		{
			var result = await Sender.Send(new GetProviderReviewStatsQuery(providerId));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("export")]
		public async Task<IActionResult> ExportCsv([FromQuery] string providerId, [FromQuery] string? filter)
		{
			var result = await Sender.Send(new ExportProviderReviewsCsvQuery(providerId, filter));

			if (!result.IsSuccess)
			{
				var response = result.ToApiResponse();
				return StatusCode(response.StatusCode, response);
			}

			var bytes = Encoding.UTF8.GetBytes(result.Value!);
			return File(bytes, "text/csv", "reviews.csv");
		}

		[HttpPost("{id}/respond")]
		public async Task<IActionResult> Respond(string id, [FromBody] RespondToReviewRequest request)
		{
			var result = await Sender.Send(new RespondToReviewCommand(id, request.Text));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}

	public sealed record RespondToReviewRequest(string Text);
}
