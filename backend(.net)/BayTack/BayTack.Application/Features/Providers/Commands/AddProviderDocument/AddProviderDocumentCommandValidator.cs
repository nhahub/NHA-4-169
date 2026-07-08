using FluentValidation;

namespace BayTack.Application.Features.Providers.Commands.AddProviderDocument
{
	public sealed class AddProviderDocumentCommandValidator : AbstractValidator<AddProviderDocumentCommand>
	{
		public AddProviderDocumentCommandValidator()
		{
			RuleFor(x => x.ProviderProfileId).NotEmpty();
			RuleFor(x => x.DocType).NotEmpty().MaximumLength(100);
			RuleFor(x => x.DocUrl).NotEmpty().MaximumLength(1000);
		}
	}
}
