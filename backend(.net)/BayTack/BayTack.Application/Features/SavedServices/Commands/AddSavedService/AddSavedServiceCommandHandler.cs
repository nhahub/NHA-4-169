using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.SavedServices.Specifications;
using BayTack.Domain.Entities.ServiceAggregate;
using BayTack.Domain.Entities.UserFeatures;

namespace BayTack.Application.Features.SavedServices.Commands.AddSavedService
{
    public sealed class AddSavedServiceCommandHandler : ICommandHandler<AddSavedServiceCommand>
    {
        private readonly IRepository<SavedService, string> _savedServiceRepository;
        private readonly IRepository<Service, string> _serviceRepository;

        public AddSavedServiceCommandHandler(
            IRepository<SavedService, string> savedServiceRepository,
            IRepository<Service, string> serviceRepository)
        {
            _savedServiceRepository = savedServiceRepository;
            _serviceRepository = serviceRepository;
        }

        public async Task<Result> Handle(AddSavedServiceCommand request, CancellationToken ct)
        {
            var service = await _serviceRepository.GetByIdAsync(request.ServiceId, ct);
            if (service is null)
                return Result.Failure("Service not found.");

            var existing = await _savedServiceRepository.FirstOrDefaultAsync(
                new SavedServiceByCustomerAndServiceSpec(request.CustomerId, request.ServiceId), ct);

            // Idempotent: saving an already-saved service is a success, not a conflict.
            if (existing is not null)
                return Result.Success();

            var saved = SavedService.Create(request.CustomerId, request.ServiceId);
            _savedServiceRepository.Add(saved);
            // NOTE: no SaveChangesAsync call here - UnitOfWorkBehavior does it automatically.

            return Result.Success();
        }
    }
}