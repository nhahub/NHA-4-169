using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Commands.VerifyProvider;

/// <summary>Marks a provider profile's verification as approved.</summary>
public sealed record VerifyProviderCommand(string ProviderProfileId) : ICommand<VerifyProviderResponse>;

public sealed record VerifyProviderResponse(
    string ProviderProfileId,
    string VerificationStatus);
