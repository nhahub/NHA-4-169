using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Common.DTO;
using BayTack.Application.Features.Messages.Commands.SendMessage;
using BayTack.Application.Features.Messages.Queries.GetConversationById;
using BayTack.Application.Features.Messages.Queries.GetMyConversations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Customer
{
	[Authorize]
	[Route("api/v{version:apiVersion}/customer/messages")]
	public class MessagesController : ApiController
	{
		public MessagesController(ISender sender, ICurrentUserService currentUser)
							: base(sender, currentUser)
		{
		}

		[HttpGet]
		[Authorize(Policy = "Permissions.Messages.CustomerView")]
		public async Task<IActionResult> GetConversations(CancellationToken ct)
		{
			var userId = CurrentUser.UserId;
			if (userId is null) return Unauthorized();

			var result = await Sender.Send(new GetMyConversationsQuery(userId), ct);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpGet("{conversationId}")]
		[Authorize(Policy = "Permissions.Messages.CustomerView")]
		public async Task<IActionResult> GetConversation(string conversationId, CancellationToken ct)
		{
			var userId = CurrentUser.UserId;
			if (userId is null) return Unauthorized();

			var result = await Sender.Send(new GetConversationByIdQuery(userId, conversationId), ct);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}

		[HttpPost("{conversationId}")]
		[Authorize(Policy = "Permissions.Messages.CustomerSend")]
		public async Task<IActionResult> SendMessage(
			string conversationId, [FromBody] SendMessageRequest payload, CancellationToken ct)
		{
			var userId = CurrentUser.UserId;
			if (userId is null) return Unauthorized();

			var result = await Sender.Send(new SendMessageCommand(userId, conversationId, payload.Text), ct);
			var response = result.ToApiResponse();
			return StatusCode(response.StatusCode, response);
		}
	}

	/// <summary>Body of POST /customer/messages/{conversationId}.</summary>
	public sealed record SendMessageRequest(string Text);
}