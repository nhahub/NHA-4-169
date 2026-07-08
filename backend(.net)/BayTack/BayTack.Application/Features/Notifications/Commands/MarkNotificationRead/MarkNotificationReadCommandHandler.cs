using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Notifications.Common;
using BayTack.Application.Features.Notifications.Specifications;
using BayTack.Domain.Entities.UserFeatures;

namespace BayTack.Application.Features.Notifications.Commands.MarkNotificationRead
{
	public sealed class MarkNotificationReadCommandHandler
		: ICommandHandler<MarkNotificationReadCommand, NotificationResponse>
	{
		private readonly IRepository<Notification, string> _notificationRepository;

		public MarkNotificationReadCommandHandler(IRepository<Notification, string> notificationRepository) =>
			_notificationRepository = notificationRepository;

		public async Task<Result<NotificationResponse>> Handle(
			MarkNotificationReadCommand request, CancellationToken ct)
		{
			var notification = await _notificationRepository.FirstOrDefaultAsync(
				new TrackedNotificationByIdForUserSpec(request.NotificationId, request.UserId), ct);

			if (notification is null)
				return Result<NotificationResponse>.Failure("Notification not found.");

			notification.MarkAsRead();
			_notificationRepository.Update(notification);
			// NOTE: no SaveChangesAsync call here - UnitOfWorkBehavior does it automatically.

			return NotificationResponse.FromEntity(notification);
		}
	}
}
