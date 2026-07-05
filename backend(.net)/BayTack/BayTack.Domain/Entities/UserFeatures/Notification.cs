using BayTack.Domain.Common.BaseEntity;
using BayTack.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Entities.UserFeatures
{

	public class Notification : BaseEntity<string>
	{
		public string UserId { get; private set; }
		public string Title { get; private set; } = string.Empty;
		public string Message { get; private set; } = string.Empty;
		public NotificationType Type { get; private set; }
		public bool IsRead { get; private set; }
		public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

		private Notification() { }

		public static Notification Create(string userId, string title, string message, NotificationType type) =>
			new() { UserId = userId, Title = title, Message = message, Type = type };

		public void MarkAsRead() => IsRead = true;
	}

}
