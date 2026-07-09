using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Providers;

namespace BayTack.Application.Features.Verification.Queries.GetVerificationQueue
{
	public sealed record GetVerificationQueueQuery(string? Status) : IQuery<List<ProviderResponse>>;

	public sealed class GetVerificationQueueQueryHandler : IQueryHandler<GetVerificationQueueQuery, List<ProviderResponse>>
	{
		private readonly IProviderRepository _providers;

		public GetVerificationQueueQueryHandler(IProviderRepository providers) => _providers = providers;

		public async Task<Result<List<ProviderResponse>>> Handle(GetVerificationQueueQuery request, CancellationToken cancellationToken) =>
			Result<List<ProviderResponse>>.Success(await _providers.GetVerificationQueueAsync(request.Status, cancellationToken));
	}
}
