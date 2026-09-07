using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.ReadStore.Models
{
	public sealed class NotificationReadModel
	{
		public string NotificationId { get; set; } = default!;
		public string UserId { get; set; } = default!;
		public string Title { get; set; } = default!;
		public string Message { get; set; } = default!;
		public string Type { get; set; } = default!;
		public bool IsRead { get; set; }
		public DateTime CreatedAtUtc { get; set; }
	}
}
