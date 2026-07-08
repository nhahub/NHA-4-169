using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Messages.Common;
using BayTack.Application.Features.Messages.Specifications;
using BayTack.Domain.Entities.Messaging;

namespace BayTack.Application.Features.Messages.Commands.SendMessage
{
	public sealed class SendMessageCommandHandler : ICommandHandler<SendMessageCommand, MessageResponse>
	{
		private readonly IRepository<Conversation, string> _conversationRepository;

		public SendMessageCommandHandler(IRepository<Conversation, string> conversationRepository) =>
			_conversationRepository = conversationRepository;

		public async Task<Result<MessageResponse>> Handle(SendMessageCommand request, CancellationToken ct)
		{
			var conversation = await _conversationRepository.FirstOrDefaultAsync(
				new TrackedConversationByIdForCustomerSpec(request.ConversationId, request.CustomerId), ct);

			if (conversation is null)
				return Result<MessageResponse>.Failure("Conversation not found.");

			var message = conversation.AddMessage(request.CustomerId, request.Text);
			_conversationRepository.Update(conversation);
			// NOTE: no SaveChangesAsync call here - UnitOfWorkBehavior does it automatically.

			return MessageResponse.FromEntity(message, request.CustomerId);
		}
	}
}
