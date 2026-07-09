using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.SystemEntities;

namespace BayTack.Application.Features.Settings.Queries.GetSettings
{
	public sealed class GetSettingsQueryHandler : IQueryHandler<GetSettingsQuery, SettingsResponse>
	{
		private readonly IRepository<PlatformSettings, string> _settings;
		private readonly IUnitOfWork _unitOfWork;

		public GetSettingsQueryHandler(IRepository<PlatformSettings, string> settings, IUnitOfWork unitOfWork)
		{
			_settings = settings;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<SettingsResponse>> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
		{
			var settings = await _settings.GetByIdAsync(PlatformSettings.SingletonId, cancellationToken);
			if (settings is null)
			{
				// First run: no settings row exists yet — create the default one so the
				// admin Settings page always has something to display.
				settings = PlatformSettings.CreateDefault();
				_settings.Add(settings);
				await _unitOfWork.SaveChangesAsync(cancellationToken);
			}

			return Result<SettingsResponse>.Success(Map(settings));
		}

		internal static SettingsResponse Map(PlatformSettings s) => new(
			s.PlatformActive, s.PlatformFee, s.DefaultUserRole, s.SupportEmail, s.MaintenanceMessage, s.UpdatedAt);
	}
}
