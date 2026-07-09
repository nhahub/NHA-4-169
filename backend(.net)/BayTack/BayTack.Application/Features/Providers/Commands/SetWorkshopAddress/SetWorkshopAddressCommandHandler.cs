using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;
using BayTack.Domain.ValueObjects;

namespace BayTack.Application.Features.Providers.Commands.SetWorkshopAddress;

public sealed class SetWorkshopAddressCommandHandler(
    IRepository<ProviderProfile, string> providerProfiles,
    IUnitOfWork unitOfWork) : ICommandHandler<SetWorkshopAddressCommand, SetWorkshopAddressResponse>
{
    public async Task<Result<SetWorkshopAddressResponse>> Handle(SetWorkshopAddressCommand command, CancellationToken cancellationToken)
    {
        var providerProfile = await providerProfiles.GetByIdAsync(command.ProviderProfileId, cancellationToken);
        if (providerProfile is null)
        {
            return Result<SetWorkshopAddressResponse>.Failure("Provider profile not found.");
        }

        var workshopAddress = Address.Create(command.Details, command.CityId, command.AreaId);
        providerProfile.SetWorkshopAddress(workshopAddress, command.UpdatedBy);

        providerProfiles.Update(providerProfile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<SetWorkshopAddressResponse>.Success(new SetWorkshopAddressResponse(
            providerProfile.Id,
            workshopAddress.Details,
            workshopAddress.CityId,
            workshopAddress.AreaId));
    }
}
