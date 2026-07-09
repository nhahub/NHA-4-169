using FluentValidation;

namespace BayTack.Application.Features.Providers.Commands.AddProviderDocument;

public sealed class AddProviderDocumentCommandValidator : AbstractValidator<AddProviderDocumentCommand>
{
    public AddProviderDocumentCommandValidator()
    {
        RuleFor(c => c.ProviderProfileId).NotEmpty();
        RuleFor(c => c.DocType).NotEmpty().MaximumLength(100);
        RuleFor(c => c.DocUrl).NotEmpty().MaximumLength(1000);
    }
}
