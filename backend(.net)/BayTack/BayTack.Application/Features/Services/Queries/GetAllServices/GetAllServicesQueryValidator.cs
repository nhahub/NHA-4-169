using FluentValidation;

namespace BayTack.Application.Features.Services.Queries.GetAllServices
{
    public sealed class GetAllServicesQueryValidator : AbstractValidator<GetAllServicesQuery>
    {
        public GetAllServicesQueryValidator()
        {
            RuleFor(x => x.Category).MaximumLength(150);
            RuleFor(x => x.Search).MaximumLength(150);
        }
    }
}