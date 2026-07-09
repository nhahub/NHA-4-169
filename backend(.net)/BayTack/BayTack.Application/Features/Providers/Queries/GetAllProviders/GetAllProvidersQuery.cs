using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Providers.Queries.GetAllProviders
{
	public sealed record GetAllProvidersQuery(string? Search, string? CategoryId, string? Status, int Page = 1, int Limit = 20)
		: IQuery<PaginatedList<ProviderResponse>>;

	public sealed class GetAllProvidersQueryHandler : IQueryHandler<GetAllProvidersQuery, PaginatedList<ProviderResponse>>
	{
		private readonly IProviderRepository _providers;

		public GetAllProvidersQueryHandler(IProviderRepository providers) => _providers = providers;

		public async Task<Result<PaginatedList<ProviderResponse>>> Handle(GetAllProvidersQuery request, CancellationToken cancellationToken)
		{
			var (items, total) = await _providers.GetAllAsync(request.Search, request.CategoryId, request.Status, request.Page, request.Limit, cancellationToken);
			return Result<PaginatedList<ProviderResponse>>.Success(new PaginatedList<ProviderResponse>(items, total, request.Page, request.Limit));
		}
	}
}
