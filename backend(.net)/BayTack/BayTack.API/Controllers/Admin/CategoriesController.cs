using BayTack.Application.Common.DTO;
using BayTack.Application.Features.Categories.Commands.CreateCategory;
using BayTack.Application.Features.Categories.Commands.DeleteCategory;
using BayTack.Application.Features.Categories.Commands.ToggleCategoryActive;
using BayTack.Application.Features.Categories.Commands.UpdateCategory;
using BayTack.Application.Features.Categories.Queries.GetAllCategories;
using BayTack.Application.Features.Categories.Queries.GetCategoryById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Admin
{
	[Authorize]
	public class CategoriesController : ApiController
	{
		[HttpGet]
		[Authorize(Policy = "Permissions.Categories.View")]
		public async Task<IActionResult> GetAll()
		{
			var result = await Sender.Send(new GetAllCategoriesQuery());
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("{id}")]
		[Authorize(Policy = "Permissions.Categories.View")]
		public async Task<IActionResult> GetById(string id)
		{
			var result = await Sender.Send(new GetCategoryByIdQuery(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost]
		[Authorize(Policy = "Permissions.Categories.Create")]
		public async Task<IActionResult> Create([FromBody] CreateCategoryRequest body)
		{
			var result = await Sender.Send(new CreateCategoryCommand(body.Name, body.Icon, body.Description));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPut("{id}")]
		[Authorize(Policy = "Permissions.Categories.Update")]
		public async Task<IActionResult> Update(string id, [FromBody] UpdateCategoryRequest body)
		{
			var result = await Sender.Send(new UpdateCategoryCommand(id, body.Name, body.Icon, body.Description, body.IsActive));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpDelete("{id}")]
		[Authorize(Policy = "Permissions.Categories.Delete")]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await Sender.Send(new DeleteCategoryCommand(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPatch("{id}/toggle")]
		[Authorize(Policy = "Permissions.Categories.Update")] 
		public async Task<IActionResult> Toggle(string id)
		{
			var result = await Sender.Send(new ToggleCategoryActiveCommand(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}

	public sealed record CreateCategoryRequest(string Name, string? Icon, string? Description);
	public sealed record UpdateCategoryRequest(string? Name, string? Icon, string? Description, bool IsActive);
}
