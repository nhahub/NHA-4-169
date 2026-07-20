using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Commands.SetWorkshopAddress;

/// <summary>Sets or updates the physical workshop address of a provider profile.</summary>
public sealed record SetWorkshopAddressCommand(
    string ProviderProfileId,
    string Details,
    string CityId,
    string? AreaId,
    string UpdatedBy) : ICommand<SetWorkshopAddressResponse>;

public sealed record SetWorkshopAddressResponse(
    string ProviderProfileId,
    string Details,
    string CityId,
    string? AreaId);
