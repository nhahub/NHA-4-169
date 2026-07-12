using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Services.Common;

namespace BayTack.Application.Features.Services.Queries.GetServiceById
{
    public sealed class GetServiceByIdQueryHandler : IQueryHandler<GetServiceByIdQuery, ServiceResponse>
    {
        private readonly IServiceRepository _serviceRepository;

        public GetServiceByIdQueryHandler(IServiceRepository serviceRepository) => _serviceRepository = serviceRepository;

        public async Task<Result<ServiceResponse>> Handle(GetServiceByIdQuery request, CancellationToken ct)
        {
            var service = await _serviceRepository.GetByIdAsync(request.ServiceId, ct);
            return service is null
                ? Result<ServiceResponse>.Failure("Service not found.")
                : service;
        }
    }
}