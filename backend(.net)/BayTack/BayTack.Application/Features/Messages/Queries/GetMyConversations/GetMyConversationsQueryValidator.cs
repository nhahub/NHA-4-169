using FluentValidation;

namespace BayTack.Application.Features.Messages.Queries.GetMyConversations
{
	public sealed class GetMyConversationsQueryValidator : AbstractValidator<GetMyConversationsQuery>
	{
		public GetMyConversationsQueryValidator()
		{
			RuleFor(x => x.CustomerId).NotEmpty();
		}
	}
}
