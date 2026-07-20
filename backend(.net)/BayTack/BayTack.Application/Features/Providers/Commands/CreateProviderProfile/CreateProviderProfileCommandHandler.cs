using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.CreateProviderProfile;

public sealed class CreateProviderProfileCommandHandler(
    IRepository<ProviderProfile, string> providerProfiles,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateProviderProfileCommand, CreateProviderProfileResponse>
{
    public async Task<Result<CreateProviderProfileResponse>> Handle(CreateProviderProfileCommand command, CancellationToken cancellationToken)
    {
        var hasExistingProfile = await providerProfiles.AnyAsync(
            new ProviderProfileByUserIdSpec(command.UserId), cancellationToken);

        if (hasExistingProfile)
        {
            return Result<CreateProviderProfileResponse>.Failure("A provider profile already exists for this user.");
        }

        var newProfile = ProviderProfile.Create(
            command.UserId,
            command.ProviderType,
            command.YearsOfExperience,
            command.Bio,
            command.CategoryId);

        providerProfiles.Add(newProfile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateProviderProfileResponse>.Success(new CreateProviderProfileResponse(
            newProfile.Id,
            newProfile.UserId,
            newProfile.ProviderType.ToString(),
            newProfile.VerificationStatus.ToString(),
            newProfile.YearsOfExperience,
            newProfile.Bio));
    }
}
