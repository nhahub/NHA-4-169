using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Commands.RejectProvider;

/// <summary>Marks a provider profile's verification as rejected.</summary>
public sealed record RejectProviderCommand(string ProviderProfileId) : ICommand<RejectProviderResponse>;

public sealed record RejectProviderResponse(
    string ProviderProfileId,
    string VerificationStatus);
