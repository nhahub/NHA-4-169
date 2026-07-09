using FluentValidation;

namespace BayTack.Application.Features.SavedServices.Queries.GetSavedServices
{
    public sealed class GetSavedServicesQueryValidator : AbstractValidator<GetSavedServicesQuery>
    {
        public GetSavedServicesQueryValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
        }
    }
}