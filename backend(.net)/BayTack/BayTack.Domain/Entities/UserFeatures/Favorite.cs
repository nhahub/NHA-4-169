using BayTack.Domain.Common.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Entities.UserFeatures
{

	public class Favorite : BaseEntity<string>
	{
		public string CustomerId { get; private set; }
		public string ProviderId { get; private set; }
		public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

		private Favorite() { }

		public static Favorite Create(string customerId, string providerId) =>
			new() { Id = Guid.NewGuid().ToString(), CustomerId = customerId, ProviderId = providerId };
	}
}
