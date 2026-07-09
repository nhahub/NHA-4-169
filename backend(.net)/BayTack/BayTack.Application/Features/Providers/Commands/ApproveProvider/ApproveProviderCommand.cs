using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Providers.Commands.ApproveProvider
{
	public sealed record ApproveProviderCommand(string Id) : ICommand<ProviderResponse>;

	public sealed class ApproveProviderCommandHandler : ICommandHandler<ApproveProviderCommand, ProviderResponse>
	{
		private readonly IProviderRepository _providers;

		public ApproveProviderCommandHandler(IProviderRepository providers) => _providers = providers;

		public async Task<Result<ProviderResponse>> Handle(ApproveProviderCommand request, CancellationToken cancellationToken)
		{
			var provider = await _providers.ApproveAsync(request.Id, cancellationToken);
			return provider is null
				? Result<ProviderResponse>.NotFound($"Provider '{request.Id}' not found")
				: Result<ProviderResponse>.Success(provider);
		}
	}
}
