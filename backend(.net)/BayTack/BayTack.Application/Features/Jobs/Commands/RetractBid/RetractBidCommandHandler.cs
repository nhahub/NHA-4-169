using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.JobAggregate;

namespace BayTack.Application.Features.Jobs.Commands.RetractBid
{
	public sealed class RetractBidCommandHandler : ICommandHandler<RetractBidCommand, RetractBidResponse>
	{
		private readonly IRepository<CustomerJob, string> _jobRepository;
		private readonly IUnitOfWork _unitOfWork;

		public RetractBidCommandHandler(IRepository<CustomerJob, string> jobRepository, IUnitOfWork unitOfWork)
		{
			_jobRepository = jobRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<RetractBidResponse>> Handle(RetractBidCommand request, CancellationToken ct)
		{
			var job = await _jobRepository.FirstOrDefaultAsync(new CustomerJobByBidIdSpec(request.BidId), ct);

			if (job is null)
				return Result<RetractBidResponse>.Failure("Bid not found.");

			try
			{
				job.WithdrawBid(request.BidId);
			}
			catch (InvalidOperationException ex)
			{
				return Result<RetractBidResponse>.Failure(ex.Message);
			}

			_jobRepository.Update(job);
			await _unitOfWork.SaveChangesAsync(ct);

			return Result<RetractBidResponse>.Success(new RetractBidResponse(request.BidId, "Withdrawn"));
		}
	}
}
