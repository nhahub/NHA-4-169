using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Commands.VerifyProvider
{
	public sealed record VerifyProviderCommand(string ProviderProfileId) : ICommand<VerifyProviderResponse>;

	public sealed record VerifyProviderResponse(
		string ProviderProfileId,
		string VerificationStatus);
}
