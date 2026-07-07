using BayTack.Application.Abstractions.Messaging;
using BayTack.Domain.Enums;

namespace BayTack.Application.Features.Providers.Commands.CreateProviderProfile
{
	public sealed record CreateProviderProfileCommand(
		string UserId,
		ProviderType ProviderType,
		int YearsOfExperience,
		string? Bio) : ICommand<CreateProviderProfileResponse>;

	public sealed record CreateProviderProfileResponse(
		string ProviderProfileId,
		string UserId,
		string ProviderType,
		string VerificationStatus,
		int YearsOfExperience,
		string? Bio);
}
