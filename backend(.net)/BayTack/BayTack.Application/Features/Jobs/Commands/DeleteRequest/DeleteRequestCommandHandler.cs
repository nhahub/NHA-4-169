using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Jobs.Specifications;
using BayTack.Domain.Entities.JobAggregate;

namespace BayTack.Application.Features.Jobs.Commands.DeleteRequest
{
    /// <summary>Soft-deletes a request. Uses CustomerJob.Cancel (sets JobStatus.Cancelled + IsDeleted)
    /// rather than a hard Remove, consistent with the soft-delete convention used elsewhere.</summary>
    public sealed class DeleteRequestCommandHandler : ICommandHandler<DeleteRequestCommand>
    {
        private readonly IRepository<CustomerJob, string> _jobRepository;

        public DeleteRequestCommandHandler(IRepository<CustomerJob, string> jobRepository) => _jobRepository = jobRepository;

        public async Task<Result> Handle(DeleteRequestCommand request, CancellationToken ct)
        {
            var job = await _jobRepository.FirstOrDefaultAsync(
                new TrackedCustomerJobByIdForUserSpec(request.JobId, request.CustomerId), ct);

            if (job is null)
                return Result.Failure("Request not found.");

            try
            {
                job.Cancel(request.CustomerId, "Deleted by customer.");
            }
            catch (InvalidOperationException ex)
            {
                return Result.Failure(ex.Message);
            }

            _jobRepository.Update(job);
            // NOTE: no SaveChangesAsync call here - UnitOfWorkBehavior does it automatically.

            return Result.Success();
        }
    }
}