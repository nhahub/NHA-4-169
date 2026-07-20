using BayTack.Application.Abstractions.Interfaces;
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
		private readonly ICurrentUserService _currentUser;

		public UpdateSettingsCommandHandler(IRepository<PlatformSettings, string> settings, IUnitOfWork unitOfWork, ICurrentUserService currentUser)
		{
			_settings = settings;
			_unitOfWork = unitOfWork;
			_currentUser = currentUser;
		}

		public async Task<Result<SettingsResponse>> Handle(UpdateSettingsCommand request, CancellationToken cancellationToken)
		{
			var settings = await _settings.GetByIdAsync(PlatformSettings.SingletonId, cancellationToken);
			var isNew = settings is null;
			settings ??= PlatformSettings.CreateDefault();

			settings.Update(request.PlatformActive, request.PlatformFee, request.DefaultUserRole, request.SupportEmail, request.MaintenanceMessage, updatedBy: _currentUser.UserId);

			if (isNew) _settings.Add(settings);
			else _settings.Update(settings);

			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result<SettingsResponse>.Success(GetSettingsQueryHandler.Map(settings));
		}
	}
}

