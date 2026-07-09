using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Settings.Commands.UpdateSettings
{
	public sealed record UpdateSettingsCommand(
		bool PlatformActive,
		decimal PlatformFee,
		string DefaultUserRole,
		string SupportEmail,
		string? MaintenanceMessage) : ICommand<SettingsResponse>;
}
