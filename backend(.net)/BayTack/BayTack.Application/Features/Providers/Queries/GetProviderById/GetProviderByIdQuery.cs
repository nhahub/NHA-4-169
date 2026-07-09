using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Providers.Queries.GetProviderById
{
	public sealed record GetProviderByIdQuery(string Id) : IQuery<ProviderResponse>;

	public sealed class GetProviderByIdQueryHandler : IQueryHandler<GetProviderByIdQuery, ProviderResponse>
	{
		private readonly IProviderRepository _providers;

		public GetProviderByIdQueryHandler(IProviderRepository providers) => _providers = providers;

		public async Task<Result<ProviderResponse>> Handle(GetProviderByIdQuery request, CancellationToken cancellationToken)
		{
			var provider = await _providers.GetByIdAsync(request.Id, cancellationToken);
			return provider is null
				? Result<ProviderResponse>.NotFound($"Provider '{request.Id}' not found")
				: Result<ProviderResponse>.Success(provider);
		}
	}
}
