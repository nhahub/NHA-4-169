using BayTack.API.Extensions;
using BayTack.Application.Features.Jobs.Commands.RetractBid;
using BayTack.Application.Features.Jobs.Commands.SubmitBid;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Providers
{
	public class BidsController : ApiController
	{
		[HttpPost]
		public async Task<IActionResult> Submit([FromBody] SubmitBidRequest request)
		{
			var command = new SubmitBidCommand(
				request.JobId,
				request.ProviderId,
				request.Price,
				request.Currency,
				request.DurationInDays,
				request.Notes);

			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpDelete("{bidId}")]
		public async Task<IActionResult> Retract(string bidId)
		{
			var result = await Sender.Send(new RetractBidCommand(bidId));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}

	public sealed record SubmitBidRequest(
		string JobId,
		string ProviderId,
		decimal Price,
		string Currency,
		int DurationInDays,
		string? Notes);
}
