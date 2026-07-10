using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.JobAggregate;
using BayTack.Domain.ValueObjects;

namespace BayTack.Application.Features.Jobs.Commands.SubmitBid
{
	public sealed class SubmitBidCommandHandler : ICommandHandler<SubmitBidCommand, SubmitBidResponse>
	{
		private readonly IRepository<CustomerJob, string> _jobRepository;
		private readonly IUnitOfWork _unitOfWork;

		public SubmitBidCommandHandler(IRepository<CustomerJob, string> jobRepository, IUnitOfWork unitOfWork)
		{
			_jobRepository = jobRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<SubmitBidResponse>> Handle(SubmitBidCommand request, CancellationToken ct)
		{
			var job = await _jobRepository.FirstOrDefaultAsync(new CustomerJobByIdForBiddingSpec(request.JobId), ct);

			if (job is null)
				return Result<SubmitBidResponse>.Failure("Job not found.");

			ProviderBid bid;
			try
			{
				var price = Money.Create(request.Price, request.Currency);
				bid = job.PlaceBid(request.ProviderId, price, request.DurationInDays, request.Notes);
			}
			catch (Exception ex) when (ex is InvalidOperationException or ArgumentException)
			{
				return Result<SubmitBidResponse>.Failure(ex.Message);
			}

			_jobRepository.Update(job);
			await _unitOfWork.SaveChangesAsync(ct);

			var response = new SubmitBidResponse(
				bid.Id,
				job.Id,
				bid.ProviderId,
				bid.ProposedPrice.Amount,
				bid.ProposedPrice.Currency,
				bid.DurationInDays,
				bid.Status.ToString());

			return Result<SubmitBidResponse>.Success(response);
		}
	}
}
