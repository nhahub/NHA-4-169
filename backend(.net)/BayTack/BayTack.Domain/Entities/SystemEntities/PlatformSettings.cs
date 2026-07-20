using BayTack.Domain.Common.BaseEntity;

namespace BayTack.Domain.Entities.SystemEntities
{
	/// <summary>
	/// Single-row table holding platform-wide settings (admin Settings page).
	/// Always has exactly one row, identified by <see cref="SingletonId"/>.
	/// </summary>
	public class PlatformSettings : AuditableEntity<string>
	{
		public const string SingletonId = "singleton";

		public bool PlatformActive { get; private set; } = true;
		public decimal PlatformFee { get; private set; }
		public string DefaultUserRole { get; private set; } = "Customer";
		public string SupportEmail { get; private set; } = string.Empty;
		public string? MaintenanceMessage { get; private set; }

		private PlatformSettings() { }

		public static PlatformSettings CreateDefault() => new()
		{
			Id = SingletonId,
			PlatformActive = true,
			PlatformFee = 0,
			DefaultUserRole = "Customer",
			SupportEmail = string.Empty,
			MaintenanceMessage = null,
		};

		public void Update(bool platformActive, decimal platformFee, string defaultUserRole, string supportEmail, string? maintenanceMessage, string? updatedBy)
		{
			PlatformActive = platformActive;
			PlatformFee = platformFee;
			DefaultUserRole = defaultUserRole;
			SupportEmail = supportEmail;
			MaintenanceMessage = maintenanceMessage;
			SetUpdated(updatedBy);
		}
	}
}
