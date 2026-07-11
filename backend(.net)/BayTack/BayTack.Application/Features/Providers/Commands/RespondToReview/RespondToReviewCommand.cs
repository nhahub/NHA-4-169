using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Commands.RespondToReview
{
	public sealed record RespondToReviewCommand(string ReviewId, string ResponseText) : ICommand<RespondToReviewResponse>;

	public sealed record RespondToReviewResponse(string ReviewId, string ProviderResponse);
}
