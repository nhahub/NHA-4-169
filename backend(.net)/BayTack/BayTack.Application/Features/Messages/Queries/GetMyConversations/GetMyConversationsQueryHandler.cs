using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Messages.Common;

namespace BayTack.Application.Features.Messages.Queries.GetMyConversations
{
	public sealed class GetMyConversationsQueryHandler
		: IQueryHandler<GetMyConversationsQuery, List<ConversationSummaryResponse>>
	{
		private readonly IConversationRepository _conversationRepository;

		public GetMyConversationsQueryHandler(IConversationRepository conversationRepository) =>
			_conversationRepository = conversationRepository;

		public async Task<Result<List<ConversationSummaryResponse>>> Handle(
			GetMyConversationsQuery request, CancellationToken ct)
		{
			var conversations = await _conversationRepository.GetConversationsForCustomerAsync(request.CustomerId, ct);
			return conversations;
		}
	}
}
