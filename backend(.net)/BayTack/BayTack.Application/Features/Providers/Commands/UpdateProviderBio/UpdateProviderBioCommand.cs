using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Commands.UpdateProviderBio;

/// <summary>Updates the free-text biography of a provider profile.</summary>
public sealed record UpdateProviderBioCommand(
    string ProviderProfileId,
    string Bio,
    string UpdatedBy) : ICommand<UpdateProviderBioResponse>;

public sealed record UpdateProviderBioResponse(
    string ProviderProfileId,
    string Bio);
