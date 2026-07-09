using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Queries.GetAllPortfolioItems
{
	public sealed class GetAllPortfolioItemsQueryHandler
		: IQueryHandler<GetAllPortfolioItemsQuery, List<PortfolioItemResponse>>
	{
		private readonly IRepository<ProviderPortfolioItem, string> _portfolioRepository;

		public GetAllPortfolioItemsQueryHandler(IRepository<ProviderPortfolioItem, string> portfolioRepository)
		{
			_portfolioRepository = portfolioRepository;
		}

		public async Task<Result<List<PortfolioItemResponse>>> Handle(
			GetAllPortfolioItemsQuery request, CancellationToken ct)
		{
			var items = await _portfolioRepository.ListAsync(
				new PortfolioItemsByProviderIdSpec(request.ProviderProfileId), ct);

			var response = items.Select(i => new PortfolioItemResponse(
				i.Id, i.ProviderProfileId, i.Title, i.Description, i.ImageUrl)).ToList();

			return Result<List<PortfolioItemResponse>>.Success(response);
		}
	}
}
