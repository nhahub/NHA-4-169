using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.UpdateProviderBio;

public sealed class UpdateProviderBioCommandHandler(
    IRepository<ProviderProfile, string> providerProfiles,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateProviderBioCommand, UpdateProviderBioResponse>
{
    public async Task<Result<UpdateProviderBioResponse>> Handle(UpdateProviderBioCommand command, CancellationToken cancellationToken)
    {
        var providerProfile = await providerProfiles.GetByIdAsync(command.ProviderProfileId, cancellationToken);
        if (providerProfile is null)
        {
            return Result<UpdateProviderBioResponse>.Failure("Provider profile not found.");
        }

        providerProfile.UpdateBio(command.Bio, command.UpdatedBy);

        providerProfiles.Update(providerProfile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UpdateProviderBioResponse>.Success(
            new UpdateProviderBioResponse(providerProfile.Id, providerProfile.Bio!));
    }
}
