using FluentValidation;

namespace BayTack.Application.Features.Providers.Commands.UpdateProviderBio
{
	public sealed class UpdateProviderBioCommandValidator : AbstractValidator<UpdateProviderBioCommand>
	{
		public UpdateProviderBioCommandValidator()
		{
			RuleFor(x => x.ProviderProfileId).NotEmpty();
			RuleFor(x => x.Bio).NotEmpty().MaximumLength(2000);
			RuleFor(x => x.UpdatedBy).NotEmpty();
		}
	}
}
