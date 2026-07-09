using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.SetAvailability;

public sealed class SetAvailabilityCommandHandler(
    IRepository<ProviderProfile, string> providerProfiles,
    IUnitOfWork unitOfWork) : ICommandHandler<SetAvailabilityCommand, SetAvailabilityResponse>
{
    public async Task<Result<SetAvailabilityResponse>> Handle(SetAvailabilityCommand command, CancellationToken cancellationToken)
    {
        var providerProfile = await providerProfiles.GetByIdAsync(command.ProviderProfileId, cancellationToken);
        if (providerProfile is null)
        {
            return Result<SetAvailabilityResponse>.Failure("Provider profile not found.");
        }

        try
        {
            providerProfile.SetAvailability(command.DayOfWeek, command.StartTime, command.EndTime);
        }
        catch (ArgumentException ex)
        {
            return Result<SetAvailabilityResponse>.Failure(ex.Message);
        }

        providerProfiles.Update(providerProfile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<SetAvailabilityResponse>.Success(new SetAvailabilityResponse(
            providerProfile.Id,
            command.DayOfWeek.ToString(),
            command.StartTime.ToString(),
            command.EndTime.ToString()));
    }
}
