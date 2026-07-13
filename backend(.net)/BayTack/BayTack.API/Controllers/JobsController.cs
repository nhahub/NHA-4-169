using BayTack.API.Extensions;
using BayTack.Application.Features.Jobs.Queries.GetBidsForJob;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers
{
	[Authorize]
	public class JobsController : ApiController
	{
		[HttpGet("{jobId}/bids")]
		[Authorize(Policy = "Permissions.Jobs.ViewBids")] // صلاحية استعراض العروض المقدمة على الطلب/المشروع
		public async Task<IActionResult> GetBidsForJob(string jobId)
		{
			var result = await Sender.Send(new GetBidsForJobQuery(jobId));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}
}
