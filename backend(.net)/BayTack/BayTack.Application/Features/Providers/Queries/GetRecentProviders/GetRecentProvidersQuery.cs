using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Providers.Queries.GetRecentProviders
{
	public sealed record GetRecentProvidersQuery(int Limit = 5) : IQuery<List<ProviderResponse>>;

	public sealed class GetRecentProvidersQueryHandler : IQueryHandler<GetRecentProvidersQuery, List<ProviderResponse>>
	{
		private readonly IProviderRepository _providers;

		public GetRecentProvidersQueryHandler(IProviderRepository providers) => _providers = providers;

		public async Task<Result<List<ProviderResponse>>> Handle(GetRecentProvidersQuery request, CancellationToken cancellationToken) =>
			Result<List<ProviderResponse>>.Success(await _providers.GetRecentAsync(request.Limit, cancellationToken));
	}
}
