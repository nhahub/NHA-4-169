using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Providers.Queries.GetProviderStats
{
	public sealed record ProviderStatsResponse(int Total, int Verified, int Pending, int Suspended);

	public sealed record GetProviderStatsQuery : IQuery<ProviderStatsResponse>;

	public sealed class GetProviderStatsQueryHandler : IQueryHandler<GetProviderStatsQuery, ProviderStatsResponse>
	{
		private readonly IProviderRepository _providers;

		public GetProviderStatsQueryHandler(IProviderRepository providers) => _providers = providers;

		public async Task<Result<ProviderStatsResponse>> Handle(GetProviderStatsQuery request, CancellationToken cancellationToken)
		{
			var (total, verified, pending, suspended) = await _providers.GetStatsAsync(cancellationToken);
			return Result<ProviderStatsResponse>.Success(new ProviderStatsResponse(total, verified, pending, suspended));
		}
	}
}
