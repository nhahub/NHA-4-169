using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Preferences.Common;
using BayTack.Application.Features.Preferences.Specifications;
using BayTack.Domain.Entities.UserFeatures;

namespace BayTack.Application.Features.Preferences.Queries.GetMyPreferences
{
    public sealed class GetMyPreferencesQueryHandler : IQueryHandler<GetMyPreferencesQuery, PreferencesResponse>
    {
        private readonly IRepository<UserPreferences, string> _repository;

        public GetMyPreferencesQueryHandler(IRepository<UserPreferences, string> repository) => _repository = repository;

        public async Task<Result<PreferencesResponse>> Handle(GetMyPreferencesQuery request, CancellationToken ct)
        {
            var existing = await _repository.FirstOrDefaultAsync(new TrackedUserPreferencesByUserIdSpec(request.UserId), ct);

            // No row yet (never saved) - return the same defaults the frontend toggles
            // were already showing, without writing anything. The row is only actually
            // created on the first PATCH.
            var preferences = existing ?? UserPreferences.CreateDefault(request.UserId);

            return PreferencesResponse.FromEntity(preferences);
        }
    }
}
