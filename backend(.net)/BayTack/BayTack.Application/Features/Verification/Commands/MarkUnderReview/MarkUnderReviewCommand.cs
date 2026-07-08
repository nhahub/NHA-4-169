using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;


namespace BayTack.Application.Features.Verification.Commands.MarkUnderReview
{
 
	public sealed record MarkUnderReviewCommand(string ProviderProfileId) : ICommand<VerificationEntryResponse>;

		
}

