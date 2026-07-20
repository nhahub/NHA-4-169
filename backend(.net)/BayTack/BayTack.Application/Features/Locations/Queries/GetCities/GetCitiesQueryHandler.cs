using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Locations.Specifications;
using BayTack.Domain.Entities.Location;

namespace BayTack.Application.Features.Locations.Queries.GetCities
{
    public sealed class GetCitiesQueryHandler : IQueryHandler<GetCitiesQuery, List<CityResponse>>
    {
        private readonly IRepository<City, string> _cityRepository;

        public GetCitiesQueryHandler(IRepository<City, string> cityRepository) => _cityRepository = cityRepository;

        public async Task<Result<List<CityResponse>>> Handle(GetCitiesQuery request, CancellationToken ct)
        {
            var cities = await _cityRepository.ListAsync(new AllCitiesWithAreasSpec(), ct);

            var response = cities.Select(c => new CityResponse(
                c.Id,
                c.Name,
                c.Areas.Select(a => new AreaResponse(a.Id, a.Name)).ToList()
            )).ToList();

            return response;
        }
    }
}
