using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Jobs.Common;
using BayTack.Application.Features.Jobs.Specifications;
using BayTack.Domain.Entities.JobAggregate;

namespace BayTack.Application.Features.Jobs.Queries.GetMyRequests
{
    public sealed class GetMyRequestsQueryHandler : IQueryHandler<GetMyRequestsQuery, List<RequestResponse>>
    {
        private readonly IRepository<CustomerJob, string> _jobRepository;

        public GetMyRequestsQueryHandler(IRepository<CustomerJob, string> jobRepository) => _jobRepository = jobRepository;

        public async Task<Result<List<RequestResponse>>> Handle(GetMyRequestsQuery request, CancellationToken ct)
        {
            var jobs = await _jobRepository.ListAsync(new CustomerJobsForUserSpec(request.CustomerId), ct);
            return jobs.Select(RequestResponse.FromEntity).ToList();
        }
    }
}