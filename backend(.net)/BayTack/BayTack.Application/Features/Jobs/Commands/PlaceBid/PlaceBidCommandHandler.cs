using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Interfaces;
using BayTack.Domain.Entities.JobAggregate;
using BayTack.Domain.ValueObjects;

namespace BayTack.Application.Features.Jobs.Commands.PlaceBid
{
	public sealed class PlaceBidCommandHandler : ICommandHandler<PlaceBidCommand, PlaceBidResponse>
	{
		private readonly IRepository<CustomerJob, int> _jobRepository;

		public PlaceBidCommandHandler(IRepository<CustomerJob, int> jobRepository) => _jobRepository = jobRepository;








		public async Task<ErrorOr<PlaceBidResponse>> Handle(PlaceBidCommand request, CancellationToken ct)
		{
			var job = await _jobRepository.FirstOrDefaultAsync(new JobWithBidsByIdSpec(request.JobId), ct);
			if (job is null)
				return DomainErrors.Job.NotFound;

			var price = Money.Create(request.ProposedPrice, request.Currency);

			try
			{
				var bid = job.PlaceBid(request.ProviderId, price, request.DurationInDays, request.Notes);
				_jobRepository.Update(job);
				return new PlaceBidResponse(bid.Id, bid.Status.ToString());
			}
			catch (InvalidOperationException ex) when (ex.Message.Contains("open for bidding", StringComparison.OrdinalIgnoreCase)
													  || ex.Message.Contains("not open", StringComparison.OrdinalIgnoreCase))
			{
				return DomainErrors.Job.NotOpenForBidding;
			}
			catch (InvalidOperationException ex) when (ex.Message.Contains("pending bid", StringComparison.OrdinalIgnoreCase))
			{
				return DomainErrors.Job.DuplicatePendingBid;
			}
		}
	}

}
