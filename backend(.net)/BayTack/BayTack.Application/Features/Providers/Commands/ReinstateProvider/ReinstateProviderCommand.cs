using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Providers.Commands.ReinstateProvider
{
	public sealed record ReinstateProviderCommand(string Id) : ICommand<ProviderResponse>;

	public sealed class ReinstateProviderCommandHandler : ICommandHandler<ReinstateProviderCommand, ProviderResponse>
	{
		private readonly IProviderRepository _providers;

		public ReinstateProviderCommandHandler(IProviderRepository providers) => _providers = providers;

		public async Task<Result<ProviderResponse>> Handle(ReinstateProviderCommand request, CancellationToken cancellationToken)
		{
			var provider = await _providers.ReinstateAsync(request.Id, cancellationToken);
			return provider is null
				? Result<ProviderResponse>.NotFound($"Provider '{request.Id}' not found")
				: Result<ProviderResponse>.Success(provider);
		}
	}
}
