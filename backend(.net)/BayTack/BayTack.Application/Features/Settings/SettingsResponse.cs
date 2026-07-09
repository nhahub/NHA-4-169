namespace BayTack.Application.Features.Settings
{
	public sealed record SettingsResponse(
		bool PlatformActive,
		decimal PlatformFee,
		string DefaultUserRole,
		string SupportEmail,
		string MaintenanceMessage,
		DateTime? UpdatedAt);
}
