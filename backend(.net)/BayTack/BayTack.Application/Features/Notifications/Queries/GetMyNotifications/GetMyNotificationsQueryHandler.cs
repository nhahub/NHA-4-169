using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Notifications.Common;
using BayTack.Application.Features.Notifications.Specifications;
using BayTack.Domain.Entities.UserFeatures;

namespace BayTack.Application.Features.Notifications.Queries.GetMyNotifications
{
	public sealed class GetMyNotificationsQueryHandler
		: IQueryHandler<GetMyNotificationsQuery, List<NotificationResponse>>
	{
		private readonly IRepository<Notification, string> _notificationRepository;

		public GetMyNotificationsQueryHandler(IRepository<Notification, string> notificationRepository) =>
			_notificationRepository = notificationRepository;

		public async Task<Result<List<NotificationResponse>>> Handle(
			GetMyNotificationsQuery request, CancellationToken ct)
		{
			var notifications = await _notificationRepository.ListAsync(
				new NotificationsForUserSpec(request.UserId), ct);

			return notifications.Select(NotificationResponse.FromEntity).ToList();
		}
	}
}
