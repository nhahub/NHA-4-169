using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.AddProviderDocument;

public sealed class AddProviderDocumentCommandHandler(
    IRepository<ProviderProfile, string> providerProfiles,
    IUnitOfWork unitOfWork) : ICommandHandler<AddProviderDocumentCommand, AddProviderDocumentResponse>
{
    public async Task<Result<AddProviderDocumentResponse>> Handle(AddProviderDocumentCommand command, CancellationToken cancellationToken)
    {
        var providerProfile = await providerProfiles.GetByIdAsync(command.ProviderProfileId, cancellationToken);
        if (providerProfile is null)
        {
            return Result<AddProviderDocumentResponse>.Failure("Provider profile not found.");
        }

        var document = providerProfile.AddDocument(command.DocType, command.DocUrl);

        providerProfiles.Update(providerProfile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AddProviderDocumentResponse>.Success(new AddProviderDocumentResponse(
            providerProfile.Id,
            document.Id,
            document.DocType,
            document.DocUrl,
            document.Status.ToString()));
    }
}
