using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.JobAggregate;

namespace BayTack.Application.Features.Jobs.Queries.GetBidsForJob
{
	public sealed class GetBidsForJobQueryHandler : IQueryHandler<GetBidsForJobQuery, List<BidResponse>>
	{
		private readonly IRepository<CustomerJob, string> _jobRepository;

		public GetBidsForJobQueryHandler(IRepository<CustomerJob, string> jobRepository)
		{
			_jobRepository = jobRepository;
		}

		public async Task<Result<List<BidResponse>>> Handle(GetBidsForJobQuery request, CancellationToken ct)
		{
			var job = await _jobRepository.FirstOrDefaultAsync(new CustomerJobWithBidsByIdSpec(request.JobId), ct);

			if (job is null)
				return Result<List<BidResponse>>.Failure("Job not found.");

			var bids = job.Bids.Select(b => new BidResponse(
				b.Id,
				b.ProviderId,
				b.ProposedPrice.Amount,
				b.ProposedPrice.Currency,
				b.DurationInDays,
				b.Notes,
				b.Status.ToString())).ToList();

			return Result<List<BidResponse>>.Success(bids);
		}
	}
}
