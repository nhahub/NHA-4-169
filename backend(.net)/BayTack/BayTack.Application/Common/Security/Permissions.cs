namespace BayTack.Application.Common.Security
{

	public static class Permissions
	{
		public const string ClaimType = "permission";

		#region Auth & Profile General
		public const string AuthGeneral = "auth.general"; // للعمليات العامة مثل تغيير الباسورد
		#endregion

		#region Categories
		public const string CategoriesView = "categories.view";
		public const string CategoriesCreate = "categories.create";
		public const string CategoriesUpdate = "categories.update";
		public const string CategoriesDelete = "categories.delete";
		#endregion

		#region Customer Requests
		public const string RequestsCustomerView = "requests.customer_view";
		public const string RequestsCustomerCreate = "requests.customer_create";
		public const string RequestsCustomerDelete = "requests.customer_delete";
		public const string RequestsCustomerAcceptOffer = "requests.customer_accept_offer";
		#endregion

		#region Customer Orders
		public const string OrdersCustomerView = "orders.customer_view";
		public const string OrdersCustomerCreate = "orders.customer_create";
		public const string OrdersCustomerCancel = "orders.customer_cancel";
		#endregion

		#region Customer Messages & Notifications & Saved
		public const string MessagesCustomerView = "messages.customer_view";
		public const string MessagesCustomerSend = "messages.customer_send";

		public const string NotificationsCustomerView = "notifications.customer_view";
		public const string NotificationsCustomerUpdate = "notifications.customer_update";

		public const string SavedCustomerView = "saved.customer_view";
		public const string SavedCustomerManage = "saved.customer_manage";
		#endregion

		#region Jobs & Bids
		public const string JobsViewBids = "jobs.view_bids";
		public const string BidsProviderSubmit = "bids.provider_submit";
		public const string BidsProviderRetract = "bids.provider_retract";
		#endregion

		#region Portfolio & Reviews
		public const string PortfolioView = "portfolio.view";
		public const string PortfolioProviderManage = "portfolio.provider_manage";

		public const string ReviewsView = "reviews.view";
		public const string ReviewsProviderManage = "reviews.provider_manage";
		#endregion

		#region Bookings
		public const string BookingsView = "bookings.view";
		public const string BookingsManage = "bookings.manage";
		#endregion

		#region Providers (Admin Management & Profile)
		public const string ProvidersProfileView = "providers.profile_view";
		public const string ProvidersProfileManage = "providers.profile_manage";
		public const string ProvidersView = "providers.view";
		public const string ProvidersViewStats = "providers.view_stats";
		public const string ProvidersUpdate = "providers.update";
		public const string ProvidersApprove = "providers.approve";
		public const string ProvidersSuspend = "providers.suspend";
		#endregion

		#region Roles & Permissions (Admin)
		public const string RolesView = "roles.view";
		public const string RolesCreate = "roles.create";
		public const string RolesUpdate = "roles.update";
		public const string RolesDelete = "roles.delete";
		public const string RolesManagePermissions = "roles.manage_permissions";
		public const string PermissionsView = "permissions.view";
		#endregion

		#region Settings (Admin & General)
		public const string SettingsView = "settings.view";
		public const string SettingsUpdate = "settings.update";
		#endregion

		#region Users (Admin)
		public const string UsersView = "users.view";
		public const string UsersCreate = "users.create";
		public const string UsersUpdate = "users.update";
		public const string UsersDeactivate = "users.deactivate";
		public const string UsersDelete = "users.delete";
		#endregion

		#region Verification (Admin)
		public const string VerificationView = "verification.view";
		public const string VerificationReview = "verification.review";
		public const string VerificationApprove = "verification.approve";
		public const string VerificationReject = "verification.reject";
		#endregion

		/// <summary>
		/// قائمة تحتوي على جميع الـ Permissions المعرفة في النظام للتسجيل والـ Seeding
		/// </summary>
		public static IReadOnlyList<PermissionDefinition> All { get; } = new List<PermissionDefinition>
		{
        // Categories
        new(CategoriesView, "View Categories", "Categories"),
		new(CategoriesCreate, "Create Category", "Categories"),
		new(CategoriesUpdate, "Update Category", "Categories"),
		new(CategoriesDelete, "Delete Category", "Categories"),

        // Requests
        new(RequestsCustomerView, "View Customer Requests", "Requests"),
		new(RequestsCustomerCreate, "Create Customer Request", "Requests"),
		new(RequestsCustomerDelete, "Delete Customer Request", "Requests"),
		new(RequestsCustomerAcceptOffer, "Accept Offer for Request", "Requests"),

        // Orders
        new(OrdersCustomerView, "View Customer Orders", "Orders"),
		new(OrdersCustomerCreate, "Create Customer Order", "Orders"),
		new(OrdersCustomerCancel, "Cancel Customer Order", "Orders"),

        // Messages, Notifications & Saved
        new(MessagesCustomerView, "View Customer Messages", "Messages"),
		new(MessagesCustomerSend, "Send Customer Message", "Messages"),
		new(NotificationsCustomerView, "View Customer Notifications", "Notifications"),
		new(NotificationsCustomerUpdate, "Update Customer Notifications", "Notifications"),
		new(SavedCustomerView, "View Saved Services", "Saved Services"),
		new(SavedCustomerManage, "Manage Saved Services", "Saved Services"),

        // Jobs & Bids
        new(JobsViewBids, "View Bids for Job", "Jobs"),
		new(BidsProviderSubmit, "Submit Bid as Provider", "Bids"),
		new(BidsProviderRetract, "Retract Bid as Provider", "Bids"),

        // Portfolio & Reviews
        new(PortfolioView, "View Portfolios", "Portfolio"),
		new(PortfolioProviderManage, "Manage Provider Portfolio", "Portfolio"),
		new(ReviewsView, "View Provider Reviews", "Reviews"),
		new(ReviewsProviderManage, "Manage Provider Reviews Response", "Reviews"),

        // Bookings
        new(BookingsView, "View Bookings", "Bookings"),
		new(BookingsManage, "Manage Bookings Status", "Bookings"),

        // Providers
        new(ProvidersProfileView, "View Provider Profile", "Providers"),
		new(ProvidersProfileManage, "Manage Provider Profile", "Providers"),
		new(ProvidersView, "View Providers List (Admin)", "Providers"),
		new(ProvidersViewStats, "View Providers Stats (Admin)", "Providers"),
		new(ProvidersUpdate, "Update Provider (Admin)", "Providers"),
		new(ProvidersApprove, "Approve Provider Account", "Providers"),
		new(ProvidersSuspend, "Suspend Provider Account", "Providers"),

        // Roles
        new(RolesView, "View Roles", "Roles"),
		new(RolesCreate, "Create Role", "Roles"),
		new(RolesUpdate, "Update Role", "Roles"),
		new(RolesDelete, "Delete Role", "Roles"),
		new(RolesManagePermissions, "Manage Role Permissions", "Roles"),
		new(PermissionsView, "View System Permissions", "Roles"),

        // Settings
        new(SettingsView, "View System Settings", "Settings"),
		new(SettingsUpdate, "Update System Settings", "Settings"),

        // Users
        new(UsersView, "View System Users", "Users"),
		new(UsersCreate, "Create System User", "Users"),
		new(UsersUpdate, "Update System User", "Users"),
		new(UsersDeactivate, "Deactivate System User", "Users"),
		new(UsersDelete, "Delete System User", "Users"),

        // Verification
        new(VerificationView, "View Verification Queue", "Verification"),
		new(VerificationReview, "Mark Verification Under Review", "Verification"),
		new(VerificationApprove, "Approve Provider Verification", "Verification"),
		new(VerificationReject, "Reject Provider Verification", "Verification")
		}.AsReadOnly();

		#region Methods to Get Permissions By Role

		/// <summary>
		/// جلب الـ Permissions الخاصة بالـ Admin (له صلاحية على كل شيء في النظام)
		/// </summary>
		public static List<string> GetAdminPermissions()
		{
			var adminPermissions = new List<string>();
			foreach (var permission in All)
			{
				adminPermissions.Add(permission.Label);
			}
			return adminPermissions;
		}

		/// <summary>
		/// جلب الـ Permissions الخاصة بالـ Customer
		/// </summary>
		public static List<string> GetCustomerPermissions()
		{
			return new List<string>
		{
			CategoriesView,
			RequestsCustomerView,
			RequestsCustomerCreate,
			RequestsCustomerDelete,
			RequestsCustomerAcceptOffer,
			OrdersCustomerView,
			OrdersCustomerCreate,
			OrdersCustomerCancel,
			MessagesCustomerView,
			MessagesCustomerSend,
			NotificationsCustomerView,
			NotificationsCustomerUpdate,
			SavedCustomerView,
			SavedCustomerManage,
			JobsViewBids,
			PortfolioView,
			ReviewsView,
			BookingsView,
			BookingsManage, // العميل يملك القدرة على تغيير حالات معينة كالإلغاء أو التأكيد
            ProvidersProfileView,
			SettingsView
		};
		}

		/// <summary>
		/// جلب الـ Permissions الخاصة بالـ Provider
		/// </summary>
		public static List<string> GetProviderPermissions()
		{
			return new List<string>
		{
			CategoriesView,
			JobsViewBids,
			BidsProviderSubmit,
			BidsProviderRetract,
			PortfolioView,
			PortfolioProviderManage,
			ReviewsView,
			ReviewsProviderManage,
			BookingsView,
			BookingsManage, // مقدم الخدمة يملك القدرة على القبول، الرفض، والإتمام
            ProvidersProfileView,
			ProvidersProfileManage,
			SettingsView
		};
		}

		#endregion


		public static bool IsValid(string permissionId) => All.Any(p => p.Id == permissionId);
	}








	/// <summary>GET /permissions -> Permission[] { id, label, group } maps directly to this.</summary>
	public sealed record PermissionDefinition(string Id, string Label, string Group);

}
