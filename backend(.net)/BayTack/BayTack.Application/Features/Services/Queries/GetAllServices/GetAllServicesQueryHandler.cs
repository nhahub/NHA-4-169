using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Services.Common;

namespace BayTack.Application.Features.Services.Queries.GetAllServices
{
    public sealed class GetAllServicesQueryHandler : IQueryHandler<GetAllServicesQuery, List<ServiceResponse>>
    {
        private readonly IServiceRepository _serviceRepository;

        public GetAllServicesQueryHandler(IServiceRepository serviceRepository) => _serviceRepository = serviceRepository;

        public async Task<Result<List<ServiceResponse>>> Handle(GetAllServicesQuery request, CancellationToken ct)
        {
            var services = await _serviceRepository.SearchAsync(request.Category, request.Search, ct);
            return services;
        }
    }
}