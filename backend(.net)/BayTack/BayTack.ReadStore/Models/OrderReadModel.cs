using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.ReadStore.Models
{
	public sealed class OrderReadModel
	{
		public string OrderId { get; set; } = default!;
		public string CustomerId { get; set; } = default!;
		public string CustomerJobId { get; set; } = default!;
		public string ServiceId { get; set; } = default!;
		public string Title { get; set; } = default!;
		public string Description { get; set; } = default!;

		public string ProviderId { get; set; } = default!;
		public string ProviderName { get; set; } = default!;

		public decimal FinalPriceAmount { get; set; }
		public string FinalPriceCurrency { get; set; } = default!;

		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string Status { get; set; } = default!;

		public DateTime CreatedAtUtc { get; set; }
		public DateTime? LastUpdatedAtUtc { get; set; }

		public List<OrderHistoryReadModel> History { get; set; } = new();
	}
}
