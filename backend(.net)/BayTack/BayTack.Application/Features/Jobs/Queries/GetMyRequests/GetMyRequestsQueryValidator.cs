using FluentValidation;

namespace BayTack.Application.Features.Jobs.Queries.GetMyRequests
{
    public sealed class GetMyRequestsQueryValidator : AbstractValidator<GetMyRequestsQuery>
    {
        public GetMyRequestsQueryValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
        }
    }
}