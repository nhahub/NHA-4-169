using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Verification;

namespace BayTack.Application.Features.Verification.Commands.MarkUnderReview;

public sealed record MarkUnderReviewCommand(string ProviderProfileId)
    : ICommand<VerificationEntryResponse>;