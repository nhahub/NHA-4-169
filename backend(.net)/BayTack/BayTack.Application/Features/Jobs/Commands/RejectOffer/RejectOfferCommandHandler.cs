using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Jobs.Common;
using BayTack.Application.Features.Jobs.Specifications;
using BayTack.Domain.Entities.JobAggregate;

namespace BayTack.Application.Features.Jobs.Commands.RejectOffer
{
    public sealed class RejectOfferCommandHandler : ICommandHandler<RejectOfferCommand, RequestResponse>
    {
        private readonly IRepository<CustomerJob, string> _jobRepository;

        public RejectOfferCommandHandler(IRepository<CustomerJob, string> jobRepository) => _jobRepository = jobRepository;

        public async Task<Result<RequestResponse>> Handle(RejectOfferCommand request, CancellationToken ct)
        {
            var job = await _jobRepository.FirstOrDefaultAsync(
                new TrackedCustomerJobByIdForUserSpec(request.JobId, request.CustomerId), ct);

            if (job is null)
                return Result<RequestResponse>.Failure("Request not found.");

            var offer = job.Bids.FirstOrDefault(b => b.Id == request.OfferId);
            if (offer is null)
                return Result<RequestResponse>.Failure("Offer not found.");

            offer.Reject();

            _jobRepository.Update(job);
            // NOTE: no SaveChangesAsync call here - UnitOfWorkBehavior does it automatically.

            return RequestResponse.FromEntity(job);
        }
    }
}
