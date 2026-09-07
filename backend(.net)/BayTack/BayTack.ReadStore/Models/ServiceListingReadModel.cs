using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.ReadStore.Models
{
	public sealed class ServiceListingReadModel
	{
		public string ListingId { get; set; } = default!;
		public string ServiceId { get; set; } = default!;
		public string Title { get; set; } = default!;
		public string Category { get; set; } = default!;
		public string? IconName { get; set; }

		public string ProviderId { get; set; } = default!;
		public string ProviderName { get; set; } = default!;

		public string BasicName { get; set; } = default!;
		public decimal BasicPrice { get; set; }
		public string BasicDescription { get; set; } = default!;
		public string BasicDelivery { get; set; } = default!;

		public string StandardName { get; set; } = default!;
		public decimal StandardPrice { get; set; }
		public string StandardDescription { get; set; } = default!;
		public string StandardDelivery { get; set; } = default!;

		public string PremiumName { get; set; } = default!;
		public decimal PremiumPrice { get; set; }
		public string PremiumDescription { get; set; } = default!;
		public string PremiumDelivery { get; set; } = default!;

		public double RatingAverage { get; set; }
		public int RatingCount { get; set; }

		public DateTime CreatedAtUtc { get; set; }
		public DateTime? LastUpdatedAtUtc { get; set; }
	}

}
