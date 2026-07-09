using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.VerifyProvider;

public sealed class VerifyProviderCommandHandler(
    IRepository<ProviderProfile, string> providerProfiles,
    IUnitOfWork unitOfWork) : ICommandHandler<VerifyProviderCommand, VerifyProviderResponse>
{
    public async Task<Result<VerifyProviderResponse>> Handle(VerifyProviderCommand command, CancellationToken cancellationToken)
    {
        var providerProfile = await providerProfiles.GetByIdAsync(command.ProviderProfileId, cancellationToken);
        if (providerProfile is null)
        {
            return Result<VerifyProviderResponse>.Failure("Provider profile not found.");
        }

        try
        {
            providerProfile.Verify();
        }
        catch (InvalidOperationException ex)
        {
            return Result<VerifyProviderResponse>.Failure(ex.Message);
        }

        providerProfiles.Update(providerProfile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<VerifyProviderResponse>.Success(
            new VerifyProviderResponse(providerProfile.Id, providerProfile.VerificationStatus.ToString()));
    }
}
