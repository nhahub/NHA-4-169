using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.ReadStore.Models
{
	public sealed class OrderHistoryReadModel
	{
		public int Id { get; set; }
		public string OrderId { get; set; } = default!;
		public string Status { get; set; } = default!;
		public string ChangedBy { get; set; } = default!;
		public DateTime ChangedAtUtc { get; set; }
	}
}
