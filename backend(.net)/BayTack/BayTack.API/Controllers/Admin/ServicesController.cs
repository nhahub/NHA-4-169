using BayTack.Application.Common.DTO;
using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Features.Services.Commands.CreateService;
using BayTack.Application.Features.Services.Commands.DeleteService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Admin
{
	[Authorize]
	[Route("api/admin/services")]
	public class ServicesController : ApiController
	{
		public ServicesController(ISender sender, ICurrentUserService currentUser)
			: base(sender, currentUser)
		{
		}

		//[HttpGet]
		//[Authorize(Policy = "Permissions.Services.View")]
		//public async Task<IActionResult> GetAll(
		//	[FromQuery] string? categoryId,
		//	[FromQuery] string? search,
		//	[FromQuery] int page = 1,
		//	[FromQuery] int limit = 20)
		//{
		//	var result = await Sender.Send(new GetAdminServicesQuery(categoryId, search, page, limit));
		//	var response = result.ToApiResponse();
		//	return StatusCode(response.StatusCode, response);
		//}

		//[HttpGet("{id}")]
		//[Authorize(Policy = "Permissions.Services.View")]
		//public async Task<IActionResult> GetById(string id)
		//{
		//	var result = await Sender.Send(new GetAdminServiceByIdQuery(id));
		//	var response = result.ToApiResponse();
		//	return StatusCode(response.StatusCode, response);
		//}

		[HttpPost]
		//[Authorize(Policy = "Permissions.Services.Create")]
		public async Task<IActionResult> Create([FromBody] CreateServiceRequest body)
		{
			var result = await Sender.Send(new CreateServiceCommand(
				body.CategoryId,
				body.Name,
				body.Description,
				body.MinPrice,
				body.MaxPrice,
				body.Currency ?? "EGP",
				body.AllowCredit,
				body.AllowInstallments,
				body.PaymentMethodIds));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		//[HttpPut("{id}")]
		//[Authorize(Policy = "Permissions.Services.Update")]
		//public async Task<IActionResult> Update(string id, [FromBody] UpdateServiceRequest body)
		//{
		//	var result = await Sender.Send(new UpdateServiceCommand(
		//		id,
		//		body.CategoryId,
		//		body.Name,
		//		body.Description,
		//		body.MinPrice,
		//		body.MaxPrice,
		//		body.Currency ?? "EGP",
		//		body.AllowCredit,
		//		body.AllowInstallments));
		//	var response = result.ToApiResponse();
		//	return StatusCode(response.StatusCode, response);
		//}

		[HttpDelete("{id}")]
		[Authorize(Policy = "Permissions.Services.Delete")]
		public async Task<IActionResult> Delete(string id)
		{
			var result = await Sender.Send(new DeleteServiceCommand(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}


	public sealed record CreateServiceRequest(
		string CategoryId,
		string Name,
		string? Description,
		decimal MinPrice,
		decimal MaxPrice,
		string? Currency,
		bool AllowCredit,
		bool AllowInstallments,
		List<string>? PaymentMethodIds);
	public sealed record UpdateServiceRequest(
		string CategoryId,
		string Name,
		string? Description,
		decimal MinPrice,
		decimal MaxPrice,
		string? Currency,
		bool AllowCredit,
		bool AllowInstallments);
}
