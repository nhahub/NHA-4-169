using FluentValidation;

namespace BayTack.Application.Features.Providers.Commands.VerifyProvider
{
	public sealed class VerifyProviderCommandValidator : AbstractValidator<VerifyProviderCommand>
	{
		public VerifyProviderCommandValidator()
		{
			RuleFor(x => x.ProviderProfileId).NotEmpty();
		}
	}
}
