using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Commands.AddProviderDocument
{
	public sealed record AddProviderDocumentCommand(
		string ProviderProfileId,
		string DocType,
		string DocUrl) : ICommand<AddProviderDocumentResponse>;

	public sealed record AddProviderDocumentResponse(
		string ProviderProfileId,
		string DocumentId,
		string DocType,
		string DocUrl,
		string Status);
}
