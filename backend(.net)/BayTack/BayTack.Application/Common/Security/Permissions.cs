namespace BayTack.Application.Common.Security
{
	public static class Permissions
	{
		public const string ClaimType = "permission";

		public const string UsersView = "users.view";
		public const string UsersManage = "users.manage";
		public const string RolesView = "roles.view";
		public const string RolesManage = "roles.manage";
		public const string JobsView = "jobs.view";
		public const string JobsManage = "jobs.manage";
		public const string ProvidersView = "providers.view";
		public const string ProvidersVerify = "providers.verify";
		public const string PaymentsView = "payments.view";
		public const string PaymentsRecord = "payments.record";

		public static IReadOnlyList<PermissionDefinition> All { get; } = new List<PermissionDefinition>
		{
		new(UsersView, "View Users", "Users"),
		new(UsersManage, "Manage Users", "Users"),
		new(RolesView, "View Roles", "Roles"),
		new(RolesManage, "Manage Roles", "Roles"),
		new(JobsView, "View Jobs", "Jobs"),
		new(JobsManage, "Manage Jobs", "Jobs"),
		new(ProvidersView, "View Providers", "Providers"),
		new(ProvidersVerify, "Verify Providers", "Providers"),
		new(PaymentsView, "View Payments", "Payments"),
		new(PaymentsRecord, "Record Payments", "Payments"),
		}.AsReadOnly();

		public static bool IsValid(string permissionId) => All.Any(p => p.Id == permissionId);
	}

	/// <summary>GET /permissions -> Permission[] { id, label, group } maps directly to this.</summary>
	public sealed record PermissionDefinition(string Id, string Label, string Group);

}
