using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Customer
{
	// Backs: Front_end/customer/app/messages  (currently mocked -> bt_c_messages)
	// Same conversation model should be reused for the provider side inbox.
	[ApiController]
	[Route("customer/messages")]
	public class MessagesController : ControllerBase
	{
		/// <summary>GET /customer/messages -> Conversation[] { id, providerId, providerName, avatar, lastMessage, unreadCount }</summary>
		[HttpGet]
		public IActionResult GetConversations()
			=> StatusCode(501, new { message = "Not implemented: GetAll conversations" });

		/// <summary>GET /customer/messages/{conversationId} -> Conversation (with full messages[])</summary>
		[HttpGet("{conversationId}")]
		public IActionResult GetConversation(string conversationId)
			=> StatusCode(501, new { message = "Not implemented: GetById conversation" });

		/// <summary>POST /customer/messages/{conversationId}  Body: { text } -> Message (appended to conversation)</summary>
		[HttpPost("{conversationId}")]
		public IActionResult SendMessage(string conversationId, [FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Send message" });
	}
}
