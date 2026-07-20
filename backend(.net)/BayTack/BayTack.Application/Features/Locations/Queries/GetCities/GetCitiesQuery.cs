using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Locations.Queries.GetCities
{
    /// <summary>Backs the city/area picker on Post-a-Request (customer) and workshop
    /// address (provider) - was previously an int CityId with nothing to pick from.</summary>
    public sealed record GetCitiesQuery : IQuery<List<CityResponse>>;

    public sealed record CityResponse(string Id, string Name, List<AreaResponse> Areas);

    public sealed record AreaResponse(string Id, string Name);
}
