using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Commands.SetAvailability;

/// <summary>Sets a weekly working-hours slot for a provider profile.</summary>
public sealed record SetAvailabilityCommand(
    string ProviderProfileId,
    DayOfWeek DayOfWeek,
    TimeSpan StartTime,
    TimeSpan EndTime) : ICommand<SetAvailabilityResponse>;

public sealed record SetAvailabilityResponse(
    string ProviderProfileId,
    string DayOfWeek,
    string StartTime,
    string EndTime);
