using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Jobs.Common;
using BayTack.Domain.Entities.JobAggregate;
using BayTack.Domain.ValueObjects;

namespace BayTack.Application.Features.Jobs.Commands.CreateRequest
{
    public sealed class CreateRequestCommandHandler : ICommandHandler<CreateRequestCommand, RequestResponse>
    {
        private readonly IRepository<CustomerJob, string> _jobRepository;

        public CreateRequestCommandHandler(IRepository<CustomerJob, string> jobRepository) => _jobRepository = jobRepository;

        public async Task<Result<RequestResponse>> Handle(CreateRequestCommand request, CancellationToken ct)
        {
            var location = Address.Create(request.LocationDetails, request.CityId, request.AreaId);

            CustomerJob job;
            try
            {
                job = CustomerJob.Create(
                    request.CustomerId,
                    request.ServiceId,
                    request.Title,
                    request.Description,
                    location,
                    request.PreferredPayment,
                    request.Budget,
                    request.Deadline);
            }
            catch (ArgumentException ex)
            {
                return Result<RequestResponse>.Failure(ex.Message);
            }

            _jobRepository.Add(job);
            // NOTE: no SaveChangesAsync call here - UnitOfWorkBehavior does it automatically.

            return RequestResponse.FromEntity(job);
        }
    }
}