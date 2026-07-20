using BayTack.API.Extensions;
using BayTack.Application.Features.Providers.Commands.AddPortfolioItem;
using BayTack.Application.Features.Providers.Commands.AddProviderDocument;
using BayTack.Application.Features.Providers.Commands.CreateProviderProfile;
using BayTack.Application.Features.Providers.Commands.RejectProvider;
using BayTack.Application.Features.Providers.Commands.SetAvailability;
using BayTack.Application.Features.Providers.Commands.SetWorkshopAddress;
using BayTack.Application.Features.Providers.Commands.UpdateProviderBio;
using BayTack.Application.Features.Providers.Commands.VerifyProvider;
using BayTack.Application.Features.Providers.Queries.GetMyOpenJobs;
using BayTack.Application.Features.Providers.Queries.GetMyProviderProfile;
using BayTack.Application.Features.Providers.Queries.GetProviderProfileById;
using BayTack.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Providers
{
	[Authorize]
	public class ProvidersController : ApiController
	{
		// NOTE: must be declared before "{id}" below so routing doesn't treat "me" as an :id value.
		[HttpGet("me")]
		[Authorize(Policy = "Permissions.Providers.ProfileView")]
		public async Task<IActionResult> GetMyProfile()
		{
			var userId = CurrentUserId ?? throw new InvalidOperationException("User ID is required.");
			var result = await Sender.Send(new GetMyProviderProfileQuery(userId));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("me/requests")]
		[Authorize(Policy = "Permissions.Providers.ProfileView")]
		public async Task<IActionResult> GetMyOpenJobs()
		{
			var userId = CurrentUserId ?? throw new InvalidOperationException("User ID is required.");
			var result = await Sender.Send(new GetMyOpenJobsQuery(userId));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost]
		[Authorize(Policy = "Permissions.Providers.ProfileManage")] // إنشاء البروفايل لأول مرة (متاح للـ Provider)
		public async Task<IActionResult> Create([FromBody] CreateProviderProfileRequest request)
		{
			var command = new CreateProviderProfileCommand(
				request.UserId,
				request.ProviderType,
				request.YearsOfExperience,
				request.Bio,
				request.CategoryId);

			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("{id}")]
		[Authorize(Policy = "Permissions.Providers.ProfileView")] // مسموح للكل لرؤية تفاصيل مقدم الخدمة
		public async Task<IActionResult> GetById(string id)
		{
			var result = await Sender.Send(new GetProviderProfileByIdQuery(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPatch("{id}/bio")]
		[Authorize(Policy = "Permissions.Providers.ProfileManage")]
		public async Task<IActionResult> UpdateBio(string id, [FromBody] UpdateProviderBioRequest request)
		{
			var command = new UpdateProviderBioCommand(id, request.Bio, request.UpdatedBy);
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost("{id}/documents")]
		[Authorize(Policy = "Permissions.Providers.ProfileManage")] // رفع الأوراق الرسمية والمستندات للتحقق
		public async Task<IActionResult> AddDocument(string id, [FromBody] AddProviderDocumentRequest request)
		{
			var command = new AddProviderDocumentCommand(id, request.DocType, request.DocUrl);
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost("{id}/verify")]
		[Authorize(Policy = "Permissions.Providers.Approve")] // حكر على الإدارة لقبول توثيق الحساب
		public async Task<IActionResult> Verify(string id)
		{
			var result = await Sender.Send(new VerifyProviderCommand(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost("{id}/reject")]
		[Authorize(Policy = "Permissions.Providers.Suspend")] // حكر على الإدارة لرفض الحساب أو تعطيله
		public async Task<IActionResult> Reject(string id)
		{
			var result = await Sender.Send(new RejectProviderCommand(id));
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost("{id}/portfolio")]
		[Authorize(Policy = "Permissions.Portfolio.ProviderManage")] // تتبع صلاحيات معرض الأعمال
		public async Task<IActionResult> AddPortfolioItem(string id, [FromBody] AddPortfolioItemRequest request)
		{
			var command = new AddPortfolioItemCommand(id, request.Title, request.Description, request.ImageUrl);
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost("{id}/availability")]
		[Authorize(Policy = "Permissions.Providers.ProfileManage")] // تحديد أوقات العمل
		public async Task<IActionResult> SetAvailability(string id, [FromBody] SetAvailabilityRequest request)
		{
			var command = new SetAvailabilityCommand(id, request.DayOfWeek, request.StartTime, request.EndTime);
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPatch("{id}/workshop-address")]
		[Authorize(Policy = "Permissions.Providers.ProfileManage")] // تحديد عنوان العمل/الورشة
		public async Task<IActionResult> SetWorkshopAddress(string id, [FromBody] SetWorkshopAddressRequest request)
		{
			var command = new SetWorkshopAddressCommand(id, request.Details, request.CityId, request.AreaId, request.UpdatedBy);
			var result = await Sender.Send(command);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}

	public sealed record CreateProviderProfileRequest(
		string UserId,
		ProviderType ProviderType,
		int YearsOfExperience,
		string? Bio,
		string? CategoryId);

	public sealed record UpdateProviderBioRequest(string Bio, string UpdatedBy);

	public sealed record AddProviderDocumentRequest(string DocType, string DocUrl);

	public sealed record AddPortfolioItemRequest(string Title, string? Description, string? ImageUrl);

	public sealed record SetAvailabilityRequest(DayOfWeek DayOfWeek, TimeSpan StartTime, TimeSpan EndTime);

	public sealed record SetWorkshopAddressRequest(string Details, string CityId, string? AreaId, string UpdatedBy);
}
