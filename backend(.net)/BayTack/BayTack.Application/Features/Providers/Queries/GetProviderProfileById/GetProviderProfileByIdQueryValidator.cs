using FluentValidation;

namespace BayTack.Application.Features.Providers.Queries.GetProviderProfileById;

public sealed class GetProviderProfileByIdQueryValidator : AbstractValidator<GetProviderProfileByIdQuery>
{
    public GetProviderProfileByIdQueryValidator()
    {
        RuleFor(q => q.ProviderProfileId).NotEmpty();
    }
}
