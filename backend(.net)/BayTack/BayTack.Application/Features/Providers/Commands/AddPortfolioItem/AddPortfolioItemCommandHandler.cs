using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.AddPortfolioItem
{
	public sealed class AddPortfolioItemCommandHandler
		: ICommandHandler<AddPortfolioItemCommand, AddPortfolioItemResponse>
	{
		private readonly IRepository<ProviderProfile, string> _providerProfileRepository;
		private readonly IUnitOfWork _unitOfWork;

		public AddPortfolioItemCommandHandler(
			IRepository<ProviderProfile, string> providerProfileRepository,
			IUnitOfWork unitOfWork)
		{
			_providerProfileRepository = providerProfileRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<AddPortfolioItemResponse>> Handle(
			AddPortfolioItemCommand request, CancellationToken ct)
		{
			var profile = await _providerProfileRepository.GetByIdAsync(request.ProviderProfileId, ct);

			if (profile is null)
				return Result<AddPortfolioItemResponse>.Failure("Provider profile not found.");

			var item = profile.AddPortfolioItem(request.Title, request.Description, request.ImageUrl);

			_providerProfileRepository.Update(profile);
			await _unitOfWork.SaveChangesAsync(ct);

			var response = new AddPortfolioItemResponse(
				profile.Id,
				item.Id,
				item.Title,
				item.Description,
				item.ImageUrl);

			return Result<AddPortfolioItemResponse>.Success(response);
		}
	}
}
