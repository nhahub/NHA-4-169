using FluentValidation;

namespace BayTack.Application.Features.Jobs.Queries.GetRequestById
{
    public sealed class GetRequestByIdQueryValidator : AbstractValidator<GetRequestByIdQuery>
    {
        public GetRequestByIdQueryValidator()
        {
            RuleFor(x => x.JobId).NotEmpty();
            RuleFor(x => x.CustomerId).NotEmpty();
        }
    }
}