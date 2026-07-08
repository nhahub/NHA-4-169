using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Messages.Common;

namespace BayTack.Application.Features.Messages.Commands.SendMessage
{
	public sealed record SendMessageCommand(string CustomerId, string ConversationId, string Text)
		: ICommand<MessageResponse>;
}
