using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Preferences.Common;

namespace BayTack.Application.Features.Preferences.Queries.GetMyPreferences
{
    public sealed record GetMyPreferencesQuery(string UserId) : IQuery<PreferencesResponse>;
}
