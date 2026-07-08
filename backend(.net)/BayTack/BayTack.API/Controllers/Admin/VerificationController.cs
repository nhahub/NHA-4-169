using BayTack.API.Extensions;
using BayTack.Application.Features.Providers.Commands.VerifyProvider;
using BayTack.Application.Features.Verification.Commands.MarkUnderReview;
using BayTack.Application.Features.Verification.Commands.Reject;
using BayTack.Application.Features.Verification.Queries.GetDetail;
using BayTack.Application.Features.Verification.Queries.GetQueue;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Admin
{
	public class VerificationController : ApiController
	{
		/// <summary>GET /verification?status= -> VerificationEntry[]</summary>
		[HttpGet]
		public async Task<IActionResult> GetQueue([FromQuery] string? status)
		{
			var result = await Sender.Send(new GetVerificationQueueQuery(status));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		/// <summary>GET /verification/{id} -> VerificationEntry (full detail)</summary>
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(string id)
		{
			var result = await Sender.Send(new GetVerificationDetailQuery(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		/// <summary>PATCH /verification/{id}/review -> VerificationEntry</summary>
		[HttpPatch("{id}/review")]
		public async Task<IActionResult> MarkUnderReview(string id)
		{
			var result = await Sender.Send(new MarkUnderReviewCommand(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		/// <summary>PATCH /verification/{id}/approve -> VerificationEntry (mirrors providers/{id}/approve)</summary>
		[HttpPatch("{id}/approve")]
		public async Task<IActionResult> Approve(string id)
		{
			var result = await Sender.Send(new VerifyProviderCommand(id)); // reuse existing command
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		/// <summary>PATCH /verification/{id}/reject  Body: { reason } -> VerificationEntry</summary>
		[HttpPatch("{id}/reject")]
		public async Task<IActionResult> Reject(string id, [FromBody] RejectVerificationRequest request)
		{
			var result = await Sender.Send(new RejectVerificationCommand(id, request.Reason));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}

	public sealed record RejectVerificationRequest(string Reason);
}
