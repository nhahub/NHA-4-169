using FluentValidation;

namespace BayTack.Application.Features.Messages.Queries.GetConversationById
{
	public sealed class GetConversationByIdQueryValidator : AbstractValidator<GetConversationByIdQuery>
	{
		public GetConversationByIdQueryValidator()
		{
			RuleFor(x => x.CustomerId).NotEmpty();
			RuleFor(x => x.ConversationId).NotEmpty();
		}
	}
}
