using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Jobs.Specifications;
using BayTack.Domain.Entities.JobAggregate;
using BayTack.Domain.ValueObjects;

namespace BayTack.Application.Features.Jobs.Commands.PlaceBid
{
    public sealed class PlaceBidCommandHandler : ICommandHandler<PlaceBidCommand, PlaceBidResponse>
    {
        private readonly IRepository<CustomerJob, string> _jobRepository;

        public PlaceBidCommandHandler(IRepository<CustomerJob, string> jobRepository) => _jobRepository = jobRepository;

        public async Task<Result<PlaceBidResponse>> Handle(PlaceBidCommand request, CancellationToken ct)
        {
            var job = await _jobRepository.FirstOrDefaultAsync(new TrackedJobWithBidsByIdSpec(request.JobId), ct);
            if (job is null)
                return Result<PlaceBidResponse>.Failure("Job not found.");

            var price = Money.Create(request.ProposedPrice, request.Currency);

            try
            {
                var bid = job.PlaceBid(request.ProviderId, price, request.DurationInDays, request.Notes);
                _jobRepository.Update(job);
                // NOTE: no SaveChangesAsync call here - UnitOfWorkBehavior does it automatically.
                return new PlaceBidResponse(bid.Id, bid.Status.ToString());
            }
            catch (InvalidOperationException ex)
            {
                return Result<PlaceBidResponse>.Failure(ex.Message);
            }
        }
    }
}