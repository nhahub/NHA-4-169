using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Preferences.Common;
using BayTack.Application.Features.Preferences.Specifications;
using BayTack.Domain.Entities.UserFeatures;

namespace BayTack.Application.Features.Preferences.Commands.UpdateMyPreferences
{
    public sealed class UpdateMyPreferencesCommandHandler : ICommandHandler<UpdateMyPreferencesCommand, PreferencesResponse>
    {
        private readonly IRepository<UserPreferences, string> _repository;

        public UpdateMyPreferencesCommandHandler(IRepository<UserPreferences, string> repository) => _repository = repository;

        public async Task<Result<PreferencesResponse>> Handle(UpdateMyPreferencesCommand request, CancellationToken ct)
        {
            var preferences = await _repository.FirstOrDefaultAsync(new TrackedUserPreferencesByUserIdSpec(request.UserId), ct);

            var isNew = preferences is null;
            preferences ??= UserPreferences.CreateDefault(request.UserId);

            preferences.Update(
                request.ShowActiveJobWidget,
                request.NotifyNewOffers,
                request.NotifyOrderUpdates,
                request.NotifyPromotions,
                request.NotifySms,
                request.ShowProfileToMessagedProviders,
                request.ShareLocationWithActiveOrders,
                request.Language,
                request.Theme);

            if (isNew) _repository.Add(preferences);
            else _repository.Update(preferences);

            return PreferencesResponse.FromEntity(preferences);
        }
    }
}
