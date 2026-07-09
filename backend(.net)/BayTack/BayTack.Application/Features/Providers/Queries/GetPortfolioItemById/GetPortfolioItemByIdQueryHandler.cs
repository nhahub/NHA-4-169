using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Providers.Queries.GetAllPortfolioItems;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Queries.GetPortfolioItemById
{
	public sealed class GetPortfolioItemByIdQueryHandler
		: IQueryHandler<GetPortfolioItemByIdQuery, PortfolioItemResponse>
	{
		private readonly IRepository<ProviderPortfolioItem, string> _portfolioRepository;

		public GetPortfolioItemByIdQueryHandler(IRepository<ProviderPortfolioItem, string> portfolioRepository)
		{
			_portfolioRepository = portfolioRepository;
		}

		public async Task<Result<PortfolioItemResponse>> Handle(
			GetPortfolioItemByIdQuery request, CancellationToken ct)
		{
			var item = await _portfolioRepository.GetByIdAsync(request.ItemId, ct);

			if (item is null)
				return Result<PortfolioItemResponse>.Failure("Portfolio item not found.");

			var response = new PortfolioItemResponse(
				item.Id, item.ProviderProfileId, item.Title, item.Description, item.ImageUrl);

			return Result<PortfolioItemResponse>.Success(response);
		}
	}
}
