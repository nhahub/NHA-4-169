using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Preferences.Common;

namespace BayTack.Application.Features.Preferences.Commands.UpdateMyPreferences
{
    public sealed record UpdateMyPreferencesCommand(
        string UserId,
        bool ShowActiveJobWidget,
        bool NotifyNewOffers,
        bool NotifyOrderUpdates,
        bool NotifyPromotions,
        bool NotifySms,
        bool ShowProfileToMessagedProviders,
        bool ShareLocationWithActiveOrders,
        string Language,
        string Theme) : ICommand<PreferencesResponse>;
}
