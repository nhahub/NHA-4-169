using FluentValidation;

namespace BayTack.Application.Features.Providers.Commands.SetWorkshopAddress;

public sealed class SetWorkshopAddressCommandValidator : AbstractValidator<SetWorkshopAddressCommand>
{
    public SetWorkshopAddressCommandValidator()
    {
        RuleFor(c => c.ProviderProfileId).NotEmpty();
        RuleFor(c => c.Details).NotEmpty().MaximumLength(500);
        RuleFor(c => c.CityId).NotEmpty();
        RuleFor(c => c.UpdatedBy).NotEmpty();
    }
}
