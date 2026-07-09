using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.SavedServices.Specifications;
using BayTack.Domain.Entities.UserFeatures;

namespace BayTack.Application.Features.SavedServices.Commands.RemoveSavedService
{
    public sealed class RemoveSavedServiceCommandHandler : ICommandHandler<RemoveSavedServiceCommand>
    {
        private readonly IRepository<SavedService, string> _savedServiceRepository;

        public RemoveSavedServiceCommandHandler(IRepository<SavedService, string> savedServiceRepository) =>
            _savedServiceRepository = savedServiceRepository;

        public async Task<Result> Handle(RemoveSavedServiceCommand request, CancellationToken ct)
        {
            var existing = await _savedServiceRepository.FirstOrDefaultAsync(
                new SavedServiceByCustomerAndServiceSpec(request.CustomerId, request.ServiceId), ct);

            // Idempotent: unsaving something that isn't saved is still a success.
            if (existing is null)
                return Result.Success();

            _savedServiceRepository.Remove(existing);
            // NOTE: no SaveChangesAsync call here - UnitOfWorkBehavior does it automatically.

            return Result.Success();
        }
    }
}