using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Messages.Common;

namespace BayTack.Application.Features.Messages.Queries.GetConversationById
{
	public sealed record GetConversationByIdQuery(string CustomerId, string ConversationId)
		: IQuery<ConversationDetailResponse>;
}
