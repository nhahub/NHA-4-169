using FluentValidation;

namespace BayTack.Application.Features.Providers.Commands.SetWorkshopAddress
{
	public sealed class SetWorkshopAddressCommandValidator : AbstractValidator<SetWorkshopAddressCommand>
	{
		public SetWorkshopAddressCommandValidator()
		{
			RuleFor(x => x.ProviderProfileId).NotEmpty();
			RuleFor(x => x.Details).NotEmpty().MaximumLength(500);
			RuleFor(x => x.CityId).GreaterThan(0);
			RuleFor(x => x.UpdatedBy).NotEmpty();
		}
	}
}
