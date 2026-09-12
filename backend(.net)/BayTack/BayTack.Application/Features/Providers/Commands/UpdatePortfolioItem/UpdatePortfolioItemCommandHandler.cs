using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.UpdatePortfolioItem
{
	public sealed class UpdatePortfolioItemCommandHandler
		: ICommandHandler<UpdatePortfolioItemCommand, UpdatePortfolioItemResponse>
	{
		private readonly IRepository<ProviderProfile, string> _providerProfileRepository;

		public UpdatePortfolioItemCommandHandler(
			IRepository<ProviderProfile, string> providerProfileRepository )
		{
			_providerProfileRepository = providerProfileRepository;
		}

		public async Task<Result<UpdatePortfolioItemResponse>> Handle(
			UpdatePortfolioItemCommand request, CancellationToken ct)
		{
			var profile = await _providerProfileRepository.FirstOrDefaultAsync(
				new ProviderProfileByPortfolioItemIdSpec(request.ItemId), ct);

			if (profile is null)
				return Result<UpdatePortfolioItemResponse>.Failure("Portfolio item not found.");

			try
			{
				profile.UpdatePortfolioItem(request.ItemId, request.Title, request.Description, request.ImageUrl);
			}
			catch (InvalidOperationException ex)
			{
				return Result<UpdatePortfolioItemResponse>.Failure(ex.Message);
			}

			_providerProfileRepository.Update(profile);

			var response = new UpdatePortfolioItemResponse(
				request.ItemId, request.Title, request.Description, request.ImageUrl);

			return Result<UpdatePortfolioItemResponse>.Success(response);
		}
	}
}
