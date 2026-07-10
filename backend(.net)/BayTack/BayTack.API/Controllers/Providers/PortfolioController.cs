using BayTack.API.Extensions;
using BayTack.Application.Features.Providers.Commands.AddPortfolioItem;
using BayTack.Application.Features.Providers.Commands.DeletePortfolioItem;
using BayTack.Application.Features.Providers.Commands.UpdatePortfolioItem;
using BayTack.Application.Features.Providers.Queries.GetAllPortfolioItems;
using BayTack.Application.Features.Providers.Queries.GetPortfolioItemById;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Providers
{
	public class PortfolioController : ApiController
	{
		[HttpGet]
		public async Task<IActionResult> GetAll([FromQuery] string providerProfileId)
		{
			var result = await Sender.Send(new GetAllPortfolioItemsQuery(providerProfileId));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(string id)
		{
			var result = await Sender.Send(new GetPortfolioItemByIdQuery(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreatePortfolioItemRequest request)
		{
			var command = new AddPortfolioItemCommand(
				request.ProviderProfileId, request.Title, request.Description, request.ImageUrl);
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(string id, [FromBody] UpdatePortfolioItemRequest request)
		{
			var command = new UpdatePortfolioItemCommand(id, request.Title, request.Description, request.ImageUrl);
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await Sender.Send(new DeletePortfolioItemCommand(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}

	public sealed record CreatePortfolioItemRequest(
		string ProviderProfileId, string Title, string? Description, string? ImageUrl);

	public sealed record UpdatePortfolioItemRequest(string Title, string? Description, string? ImageUrl);
}
