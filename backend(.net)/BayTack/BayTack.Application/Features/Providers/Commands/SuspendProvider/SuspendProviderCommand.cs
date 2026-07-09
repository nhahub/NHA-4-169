using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Providers.Commands.SuspendProvider
{
	public sealed record SuspendProviderCommand(string Id, string? Reason) : ICommand<ProviderResponse>;

	public sealed class SuspendProviderCommandHandler : ICommandHandler<SuspendProviderCommand, ProviderResponse>
	{
		private readonly IProviderRepository _providers;

		public SuspendProviderCommandHandler(IProviderRepository providers) => _providers = providers;

		public async Task<Result<ProviderResponse>> Handle(SuspendProviderCommand request, CancellationToken cancellationToken)
		{
			var provider = await _providers.SuspendAsync(request.Id, request.Reason, cancellationToken);
			return provider is null
				? Result<ProviderResponse>.NotFound($"Provider '{request.Id}' not found")
				: Result<ProviderResponse>.Success(provider);
		}
	}
}
