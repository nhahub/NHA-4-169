using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Jobs.Common;
using BayTack.Application.Features.Jobs.Specifications;
using BayTack.Domain.Entities.JobAggregate;

namespace BayTack.Application.Features.Jobs.Queries.GetRequestById
{
    public sealed class GetRequestByIdQueryHandler : IQueryHandler<GetRequestByIdQuery, RequestResponse>
    {
        private readonly IRepository<CustomerJob, string> _jobRepository;

        public GetRequestByIdQueryHandler(IRepository<CustomerJob, string> jobRepository) => _jobRepository = jobRepository;

        public async Task<Result<RequestResponse>> Handle(GetRequestByIdQuery request, CancellationToken ct)
        {
            var job = await _jobRepository.FirstOrDefaultAsync(
                new CustomerJobByIdForUserSpec(request.JobId, request.CustomerId), ct);

            if (job is null)
                return Result<RequestResponse>.Failure("Request not found.");

            return RequestResponse.FromEntity(job);
        }
    }
}