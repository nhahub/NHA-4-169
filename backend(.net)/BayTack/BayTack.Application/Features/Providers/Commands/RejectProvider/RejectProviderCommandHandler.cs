using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.RejectProvider;

public sealed class RejectProviderCommandHandler(
    IRepository<ProviderProfile, string> providerProfiles,
    IUnitOfWork unitOfWork) : ICommandHandler<RejectProviderCommand, RejectProviderResponse>
{
    public async Task<Result<RejectProviderResponse>> Handle(RejectProviderCommand command, CancellationToken cancellationToken)
    {
        var providerProfile = await providerProfiles.GetByIdAsync(command.ProviderProfileId, cancellationToken);
        if (providerProfile is null)
        {
            return Result<RejectProviderResponse>.Failure("Provider profile not found.");
        }

        providerProfile.Reject();

        providerProfiles.Update(providerProfile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<RejectProviderResponse>.Success(
            new RejectProviderResponse(providerProfile.Id, providerProfile.VerificationStatus.ToString()));
    }
}
