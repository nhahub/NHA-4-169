using BayTack.Domain.Entities.UserFeatures;

namespace BayTack.Application.Features.Preferences.Common
{
    public sealed record PreferencesResponse(
        bool ShowActiveJobWidget,
        bool NotifyNewOffers,
        bool NotifyOrderUpdates,
        bool NotifyPromotions,
        bool NotifySms,
        bool ShowProfileToMessagedProviders,
        bool ShareLocationWithActiveOrders,
        string Language,
        string Theme)
    {
        public static PreferencesResponse FromEntity(UserPreferences p) => new(
            p.ShowActiveJobWidget,
            p.NotifyNewOffers,
            p.NotifyOrderUpdates,
            p.NotifyPromotions,
            p.NotifySms,
            p.ShowProfileToMessagedProviders,
            p.ShareLocationWithActiveOrders,
            p.Language,
            p.Theme);
    }
}
