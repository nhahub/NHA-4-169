using FluentValidation;

namespace BayTack.Application.Features.Providers.Commands.RejectProvider
{
	public sealed class RejectProviderCommandValidator : AbstractValidator<RejectProviderCommand>
	{
		public RejectProviderCommandValidator()
		{
			RuleFor(x => x.ProviderProfileId).NotEmpty();
		}
	}
}
