using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.SavedServices.Common;
using BayTack.Application.Features.SavedServices.Specifications;
using BayTack.Domain.Entities.ServiceAggregate;
using BayTack.Domain.Entities.UserFeatures;

namespace BayTack.Application.Features.SavedServices.Queries.GetSavedServices
{
    /// <summary>
    /// Two lookups because a Specification can only target one aggregate (see ServicesByIdsSpec):
    /// first the bookmark rows for the customer, then the full Service entities for those ids.
    /// </summary>
    public sealed class GetSavedServicesQueryHandler : IQueryHandler<GetSavedServicesQuery, List<ServiceResponse>>
    {
        private readonly IRepository<SavedService, string> _savedServiceRepository;
        private readonly IRepository<Service, string> _serviceRepository;

        public GetSavedServicesQueryHandler(
            IRepository<SavedService, string> savedServiceRepository,
            IRepository<Service, string> serviceRepository)
        {
            _savedServiceRepository = savedServiceRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task<Result<List<ServiceResponse>>> Handle(GetSavedServicesQuery request, CancellationToken ct)
        {
            var saved = await _savedServiceRepository.ListAsync(new SavedServicesForUserSpec(request.CustomerId), ct);
            if (saved.Count == 0)
                return new List<ServiceResponse>();

            var serviceIds = saved.Select(s => s.ServiceId).ToList();
            var services = await _serviceRepository.ListAsync(new ServicesByIdsSpec(serviceIds), ct);

            // Preserve save order (newest bookmark first) rather than whatever order the DB returns services in.
            var byId = services.ToDictionary(s => s.Id);
            var ordered = serviceIds
                .Where(byId.ContainsKey)
                .Select(id => ServiceResponse.FromEntity(byId[id]))
                .ToList();

            return ordered;
        }
    }
}