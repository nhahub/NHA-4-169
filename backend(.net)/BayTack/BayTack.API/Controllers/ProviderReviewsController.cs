using BayTack.API.Extensions;
using BayTack.Application.Features.Providers.Commands.RespondToReview;
using BayTack.Application.Features.Providers.Queries.ExportProviderReviewsCsv;
using BayTack.Application.Features.Providers.Queries.GetProviderReviews;
using BayTack.Application.Features.Providers.Queries.GetProviderReviewStats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BayTack.API.Controllers
{
	[Authorize]
	public class ProviderReviewsController : ApiController
	{
		[HttpGet]
		[Authorize(Policy = "Permissions.Reviews.View")] // مسموح للكل لرؤية تقييمات مقدم الخدمة لتسهيل اختيار العميل
		public async Task<IActionResult> GetReviews(
			[FromQuery] string providerId, [FromQuery] string? filter, [FromQuery] int page = 1, [FromQuery] int limit = 10)
		{
			var result = await Sender.Send(new GetProviderReviewsQuery(providerId, filter, page, limit));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("stats")]
		[Authorize(Policy = "Permissions.Reviews.View")] // مسموح للكل لرؤية إحصائيات التقييمات (مثلاً النجوم ونسب الرضا)
		public async Task<IActionResult> GetStats([FromQuery] string providerId)
		{
			var result = await Sender.Send(new GetProviderReviewStatsQuery(providerId));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("export")]
		[Authorize(Policy = "Permissions.Reviews.ProviderManage")] // متاح للـ Provider لتحميل بياناته أو الـ Admin للرقابة
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
		[Authorize(Policy = "Permissions.Reviews.ProviderManage")] // صلاحية رد الـ Provider على مراجعة العميل
		public async Task<IActionResult> Respond(string id, [FromBody] RespondToReviewRequest request)
		{
			var result = await Sender.Send(new RespondToReviewCommand(id, request.Text));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}
	public sealed record RespondToReviewRequest(string Text);
}
