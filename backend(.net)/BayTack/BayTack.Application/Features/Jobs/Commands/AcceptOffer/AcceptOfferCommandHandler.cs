using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Jobs.Common;
using BayTack.Application.Features.Jobs.Specifications;
using BayTack.Domain.Entities.JobAggregate;
using BayTack.Domain.Entities.OrderAggregate;

namespace BayTack.Application.Features.Jobs.Commands.AcceptOffer
{
    /// <summary>
    /// Accepts a bid on a request and turns it into an Order.
    /// The original design (see project notes) wanted Order-creation done asynchronously via a
    /// domain-event + outbox handler so this command would only touch the CustomerJob aggregate.
    /// This codebase doesn't have a domain-event/outbox pipeline yet (no IHasDomainEvents, no
    /// OutboxMessage, no MediatR notification publishing from SaveChanges - see
    /// Common/Behaviors/UnitOfWorkBehavior.cs), so the Order is created inline here for now.
    /// Flagging rather than silently building an outbox pipeline that wasn't asked for.
    /// </summary>
    public sealed class AcceptOfferCommandHandler : ICommandHandler<AcceptOfferCommand, RequestResponse>
    {
        private readonly IRepository<CustomerJob, string> _jobRepository;
        private readonly IRepository<Order, string> _orderRepository;

        public AcceptOfferCommandHandler(
            IRepository<CustomerJob, string> jobRepository,
            IRepository<Order, string> orderRepository)
        {
            _jobRepository = jobRepository;
            _orderRepository = orderRepository;
        }

        public async Task<Result<RequestResponse>> Handle(AcceptOfferCommand request, CancellationToken ct)
        {
            var job = await _jobRepository.FirstOrDefaultAsync(
                new TrackedCustomerJobByIdForUserSpec(request.JobId, request.CustomerId), ct);

            if (job is null)
                return Result<RequestResponse>.Failure("Request not found.");

            var offer = job.Bids.FirstOrDefault(b => b.Id == request.OfferId);
            if (offer is null)
                return Result<RequestResponse>.Failure("Offer not found.");

            try
            {
                job.AcceptBid(request.OfferId);
            }
            catch (InvalidOperationException ex)
            {
                return Result<RequestResponse>.Failure(ex.Message);
            }

            var order = Order.Create(job.Id, offer.ProviderId, offer.ProposedPrice, DateTime.UtcNow, request.CustomerId);

            _jobRepository.Update(job);
            _orderRepository.Add(order);
            // NOTE: no SaveChangesAsync call here - UnitOfWorkBehavior does it automatically.
            // Both aggregates are saved in the same transaction since this handler runs inside
            // a single UnitOfWorkBehavior-triggered SaveChanges call.

            return RequestResponse.FromEntity(job);
        }
    }
}