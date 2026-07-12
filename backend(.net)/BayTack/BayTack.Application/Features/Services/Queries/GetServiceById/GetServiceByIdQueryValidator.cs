using FluentValidation;

namespace BayTack.Application.Features.Services.Queries.GetServiceById
{
    public sealed class GetServiceByIdQueryValidator : AbstractValidator<GetServiceByIdQuery>
    {
        public GetServiceByIdQueryValidator()
        {
            RuleFor(x => x.ServiceId).NotEmpty();
        }
    }
}