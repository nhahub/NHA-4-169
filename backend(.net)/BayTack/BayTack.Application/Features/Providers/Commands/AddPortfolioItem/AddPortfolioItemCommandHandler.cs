using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.AddPortfolioItem;

public sealed class AddPortfolioItemCommandHandler(
    IRepository<ProviderProfile, string> providerProfiles,
    IUnitOfWork unitOfWork) : ICommandHandler<AddPortfolioItemCommand, AddPortfolioItemResponse>
{
    public async Task<Result<AddPortfolioItemResponse>> Handle(AddPortfolioItemCommand command, CancellationToken cancellationToken)
    {
        var providerProfile = await providerProfiles.GetByIdAsync(command.ProviderProfileId, cancellationToken);
        if (providerProfile is null)
        {
            return Result<AddPortfolioItemResponse>.Failure("Provider profile not found.");
        }

        var portfolioItem = providerProfile.AddPortfolioItem(command.Title, command.Description, command.ImageUrl);

        providerProfiles.Update(providerProfile);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<AddPortfolioItemResponse>.Success(new AddPortfolioItemResponse(
            providerProfile.Id,
            portfolioItem.Id,
            portfolioItem.Title,
            portfolioItem.Description,
            portfolioItem.ImageUrl));
    }
}
