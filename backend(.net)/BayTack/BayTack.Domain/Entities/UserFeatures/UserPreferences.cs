using BayTack.Domain.Common.BaseEntity;

namespace BayTack.Domain.Entities.UserFeatures
{
    /// <summary>
    /// One row per user, holding the toggle/language/appearance preferences shown on
    /// Front_end/customer/app/settings (built and left disabled pending this entity -
    /// see the "aren't saved yet" note that was on that page). Shared across portals
    /// (Customer/Provider/Admin all use the same shape) rather than one entity per
    /// role, since the fields don't actually differ by role.
    /// </summary>
    public class UserPreferences : BaseEntity<string>
    {
        public string UserId { get; private set; }

        // General
        public bool ShowActiveJobWidget { get; private set; } = true;

        // Notifications
        public bool NotifyNewOffers { get; private set; } = true;
        public bool NotifyOrderUpdates { get; private set; } = true;
        public bool NotifyPromotions { get; private set; }
        public bool NotifySms { get; private set; }

        // Privacy
        public bool ShowProfileToMessagedProviders { get; private set; } = true;
        public bool ShareLocationWithActiveOrders { get; private set; } = true;

        // Locale / appearance
        public string Language { get; private set; } = "en";
        public string Theme { get; private set; } = "light";

        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        private UserPreferences() { UserId = string.Empty; }

        /// <summary>Creates a row with the same defaults the frontend toggles were already
        /// showing (on/on/off/off for notifications, on/on for privacy) so a first-time
        /// GET doesn't visually reset switches the user never touched.</summary>
        public static UserPreferences CreateDefault(string userId) => new()
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
        };

        public void Update(
            bool showActiveJobWidget,
            bool notifyNewOffers,
            bool notifyOrderUpdates,
            bool notifyPromotions,
            bool notifySms,
            bool showProfileToMessagedProviders,
            bool shareLocationWithActiveOrders,
            string language,
            string theme)
        {
            ShowActiveJobWidget = showActiveJobWidget;
            NotifyNewOffers = notifyNewOffers;
            NotifyOrderUpdates = notifyOrderUpdates;
            NotifyPromotions = notifyPromotions;
            NotifySms = notifySms;
            ShowProfileToMessagedProviders = showProfileToMessagedProviders;
            ShareLocationWithActiveOrders = shareLocationWithActiveOrders;
            Language = string.IsNullOrWhiteSpace(language) ? "en" : language;
            Theme = string.IsNullOrWhiteSpace(theme) ? "light" : theme;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
