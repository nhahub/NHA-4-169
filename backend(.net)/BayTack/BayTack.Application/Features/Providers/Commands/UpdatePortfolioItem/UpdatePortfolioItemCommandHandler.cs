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
		private readonly IUnitOfWork _unitOfWork;

		public UpdatePortfolioItemCommandHandler(
			IRepository<ProviderProfile, string> providerProfileRepository,
			IUnitOfWork unitOfWork)
		{
			_providerProfileRepository = providerProfileRepository;
			_unitOfWork = unitOfWork;
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
			await _unitOfWork.SaveChangesAsync(ct);

			var response = new UpdatePortfolioItemResponse(
				request.ItemId, request.Title, request.Description, request.ImageUrl);

			return Result<UpdatePortfolioItemResponse>.Success(response);
		}
	}
}
