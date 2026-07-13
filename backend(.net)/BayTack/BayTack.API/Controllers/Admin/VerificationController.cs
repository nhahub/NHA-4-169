using BayTack.API.Extensions;
using BayTack.Application.Features.Providers.Commands.VerifyProvider;
using BayTack.Application.Features.Verification.Commands.MarkUnderReview;
using BayTack.Application.Features.Verification.Commands.Reject;
using BayTack.Application.Features.Verification.Queries.GetDetail;
using BayTack.Application.Features.Verification.Queries.GetQueue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Admin
{
	[Authorize]
	public class VerificationController : ApiController
	{
		[HttpGet]
		[Authorize(Policy = "Permissions.Verification.View")]
		public async Task<IActionResult> GetQueue([FromQuery] string? status)
		{
			var result = await Sender.Send(new GetVerificationQueueQuery(status));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("{id}")]
		[Authorize(Policy = "Permissions.Verification.View")]
		public async Task<IActionResult> GetById(string id)
		{
			var result = await Sender.Send(new GetVerificationDetailQuery(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPatch("{id}/review")]
		[Authorize(Policy = "Permissions.Verification.Review")] // صلاحية بدء المراجعة وتغيير الحالة لـ Under Review
		public async Task<IActionResult> MarkUnderReview(string id)
		{
			var result = await Sender.Send(new MarkUnderReviewCommand(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPatch("{id}/approve")]
		[Authorize(Policy = "Permissions.Verification.Approve")] // صلاحية القبول النهائي والتوثيق
		public async Task<IActionResult> Approve(string id)
		{
			var result = await Sender.Send(new VerifyProviderCommand(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPatch("{id}/reject")]
		[Authorize(Policy = "Permissions.Verification.Reject")] // صلاحية الرفض مع ذكر السبب
		public async Task<IActionResult> Reject(string id, [FromBody] RejectVerificationRequest request)
		{
			var result = await Sender.Send(new RejectVerificationCommand(id, request.Reason));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}
	public sealed record RejectVerificationRequest(string Reason);
}
