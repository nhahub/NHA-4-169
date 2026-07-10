using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.DeletePortfolioItem
{
	public sealed class DeletePortfolioItemCommandHandler
		: ICommandHandler<DeletePortfolioItemCommand, DeletePortfolioItemResponse>
	{
		private readonly IRepository<ProviderProfile, string> _providerProfileRepository;
		private readonly IUnitOfWork _unitOfWork;

		public DeletePortfolioItemCommandHandler(
			IRepository<ProviderProfile, string> providerProfileRepository,
			IUnitOfWork unitOfWork)
		{
			_providerProfileRepository = providerProfileRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<DeletePortfolioItemResponse>> Handle(
			DeletePortfolioItemCommand request, CancellationToken ct)
		{
			var profile = await _providerProfileRepository.FirstOrDefaultAsync(
				new ProviderProfileByPortfolioItemIdSpec(request.ItemId), ct);

			if (profile is null)
				return Result<DeletePortfolioItemResponse>.Failure("Portfolio item not found.");

			try
			{
				profile.RemovePortfolioItem(request.ItemId);
			}
			catch (InvalidOperationException ex)
			{
				return Result<DeletePortfolioItemResponse>.Failure(ex.Message);
			}

			_providerProfileRepository.Update(profile);
			await _unitOfWork.SaveChangesAsync(ct);

			return Result<DeletePortfolioItemResponse>.Success(new DeletePortfolioItemResponse(request.ItemId, true));
		}
	}
}
