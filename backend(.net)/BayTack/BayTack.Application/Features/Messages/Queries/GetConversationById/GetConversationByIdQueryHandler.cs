using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Messages.Common;
using BayTack.Application.Features.Messages.Specifications;
using BayTack.Domain.Entities.Messaging;

namespace BayTack.Application.Features.Messages.Queries.GetConversationById
{
	public sealed class GetConversationByIdQueryHandler
		: IQueryHandler<GetConversationByIdQuery, ConversationDetailResponse>
	{
		private readonly IRepository<Conversation, string> _conversationRepository;
		private readonly IConversationRepository _conversationReadRepository;

		public GetConversationByIdQueryHandler(
			IRepository<Conversation, string> conversationRepository,
			IConversationRepository conversationReadRepository)
		{
			_conversationRepository = conversationRepository;
			_conversationReadRepository = conversationReadRepository;
		}

		public async Task<Result<ConversationDetailResponse>> Handle(
			GetConversationByIdQuery request, CancellationToken ct)
		{
			var conversation = await _conversationRepository.FirstOrDefaultAsync(
				new ConversationByIdForCustomerSpec(request.ConversationId, request.CustomerId), ct);

			if (conversation is null)
				return Result<ConversationDetailResponse>.Failure("Conversation not found.");

			var providerName = await _conversationReadRepository.GetDisplayNameAsync(conversation.ProviderId, ct)
				?? "Unknown provider";

			// NOTE: avatar isn't a real column on AppUser yet - same gap flagged in
			// NotificationResponse for icons. Returning null until that column exists.
			return ConversationDetailResponse.FromEntity(conversation, providerName, null, request.CustomerId);
		}
	}
}
