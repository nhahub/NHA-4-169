using BayTack.Application.Abstractions.Messaging;
using BayTack.Domain.Enums;

namespace BayTack.Application.Features.Providers.Commands.CreateProviderProfile;

/// <summary>Creates a brand new provider profile for a user who does not already have one.</summary>
public sealed record CreateProviderProfileCommand(
    string UserId,
    ProviderType ProviderType,
    int YearsOfExperience,
    string? Bio) : ICommand<CreateProviderProfileResponse>;

public sealed record CreateProviderProfileResponse(
    string ProviderProfileId,
    string UserId,
    string ProviderType,
    string VerificationStatus,
    int YearsOfExperience,
    string? Bio);
