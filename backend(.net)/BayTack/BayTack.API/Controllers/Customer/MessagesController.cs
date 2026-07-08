using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Common.DTO;
using BayTack.Application.Features.Messages.Commands.SendMessage;
using BayTack.Application.Features.Messages.Queries.GetConversationById;
using BayTack.Application.Features.Messages.Queries.GetMyConversations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BayTack.API.Controllers.Customer
{
    // Backs: Front_end/customer/app/messages  (was mocked -> bt_c_messages)
    // Same Conversation/Message model is reused for the provider side inbox - a Provider
    // controller can call GetMyConversationsQuery/GetConversationByIdQuery/SendMessageCommand
    // with (customerId: conversation.CustomerId, providerId acting as sender) the same way.
    [Authorize]
    [Route("customer/messages")]
    public class MessagesController : ApiController
    {
        private readonly ICurrentUserService _currentUser;

        public MessagesController(ICurrentUserService currentUser) => _currentUser = currentUser;

        /// <summary>GET /customer/messages -> Conversation[] { id, providerId, providerName, avatar, lastMessage, unreadCount }</summary>
        [HttpGet]
        public async Task<IActionResult> GetConversations(CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new GetMyConversationsQuery(userId), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>GET /customer/messages/{conversationId} -> Conversation (with full messages[])</summary>
        [HttpGet("{conversationId}")]
        public async Task<IActionResult> GetConversation(string conversationId, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new GetConversationByIdQuery(userId, conversationId), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>POST /customer/messages/{conversationId}  Body: { text } -> Message (appended to conversation)</summary>
        [HttpPost("{conversationId}")]
        public async Task<IActionResult> SendMessage(
            string conversationId, [FromBody] SendMessageRequest payload, CancellationToken ct)
        {
            var userId = _currentUser.UserId;
            if (userId is null) return Unauthorized();

            var result = await Sender.Send(new SendMessageCommand(userId, conversationId, payload.Text), ct);
            var response = result.ToApiResponse();
            return StatusCode(response.StatusCode, response);
        }
    }

    /// <summary>Body of POST /customer/messages/{conversationId}.</summary>
    public sealed record SendMessageRequest(string Text);
}