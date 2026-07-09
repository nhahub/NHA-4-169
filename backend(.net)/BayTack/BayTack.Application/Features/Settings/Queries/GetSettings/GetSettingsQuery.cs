using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Settings.Queries.GetSettings
{
	public sealed record GetSettingsQuery : IQuery<SettingsResponse>;
}
