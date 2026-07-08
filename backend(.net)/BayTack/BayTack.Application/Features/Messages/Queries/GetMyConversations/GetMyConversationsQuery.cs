using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Messages.Common;

namespace BayTack.Application.Features.Messages.Queries.GetMyConversations
{
	public sealed record GetMyConversationsQuery(string CustomerId) : IQuery<List<ConversationSummaryResponse>>;
}
