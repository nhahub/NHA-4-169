using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Commands.RejectProvider
{
	public sealed record RejectProviderCommand(string ProviderProfileId) : ICommand<RejectProviderResponse>;

	public sealed record RejectProviderResponse(
		string ProviderProfileId,
		string VerificationStatus);
}
