using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Notifications.Specifications;
using BayTack.Domain.Entities.UserFeatures;

namespace BayTack.Application.Features.Notifications.Commands.MarkAllNotificationsRead
{
	public sealed class MarkAllNotificationsReadCommandHandler : ICommandHandler<MarkAllNotificationsReadCommand>
	{
		private readonly IRepository<Notification, string> _notificationRepository;

		public MarkAllNotificationsReadCommandHandler(IRepository<Notification, string> notificationRepository) =>
			_notificationRepository = notificationRepository;

		public async Task<Result> Handle(MarkAllNotificationsReadCommand request, CancellationToken ct)
		{
			var unread = await _notificationRepository.ListAsync(
				new TrackedUnreadNotificationsForUserSpec(request.UserId), ct);

			foreach (var notification in unread)
			{
				notification.MarkAsRead();
				_notificationRepository.Update(notification);
			}
			// NOTE: no SaveChangesAsync call here - UnitOfWorkBehavior does it automatically.

			return Result.Success();
		}
	}
}
