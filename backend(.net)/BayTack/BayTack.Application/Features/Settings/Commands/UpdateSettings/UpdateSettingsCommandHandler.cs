using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Settings.Queries.GetSettings;
using BayTack.Domain.Entities.SystemEntities;

namespace BayTack.Application.Features.Settings.Commands.UpdateSettings
{
	public sealed class UpdateSettingsCommandHandler : ICommandHandler<UpdateSettingsCommand, SettingsResponse>
	{
		private readonly IRepository<PlatformSettings, string> _settings;
		private readonly IUnitOfWork _unitOfWork;

		public UpdateSettingsCommandHandler(IRepository<PlatformSettings, string> settings, IUnitOfWork unitOfWork)
		{
			_settings = settings;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<SettingsResponse>> Handle(UpdateSettingsCommand request, CancellationToken cancellationToken)
		{
			var settings = await _settings.GetByIdAsync(PlatformSettings.SingletonId, cancellationToken);
			var isNew = settings is null;
			settings ??= PlatformSettings.CreateDefault();

			// TODO: pass the real signed-in user id once JWT auth is wired up.
			settings.Update(request.PlatformActive, request.PlatformFee, request.DefaultUserRole, request.SupportEmail, request.MaintenanceMessage, updatedBy: null);

			if (isNew) _settings.Add(settings);
			else _settings.Update(settings);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result<SettingsResponse>.Success(GetSettingsQueryHandler.Map(settings));
		}
	}
}
