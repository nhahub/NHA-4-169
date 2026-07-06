using BayTack.Domain.Common.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Entities.ProviderAggregate
{
	public class ProviderAvailability : BaseEntity<string>
	{
		public string ProviderProfileId { get; private set; }
		public DayOfWeek DayOfWeek { get; private set; }
		public TimeSpan StartTime { get; private set; }
		public TimeSpan EndTime { get; private set; }

		private ProviderAvailability() { }

		internal static ProviderAvailability Create(string providerProfileId, DayOfWeek day, TimeSpan start, TimeSpan end) =>
			new() { ProviderProfileId = providerProfileId, DayOfWeek = day, StartTime = start, EndTime = end };
	}

}
