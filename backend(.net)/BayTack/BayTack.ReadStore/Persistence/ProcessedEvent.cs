using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.ReadStore.Persistence
{
	public sealed class ProcessedEvent
	{
		public Guid EventId { get; set; }
		public string EventType { get; set; } = default!;
		public DateTime ProcessedAtUtc { get; set; }
	}

}
