using BayTack.Domain.Common.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Entities.JobAggregate
{
	public class JobImage : BaseEntity<string>
	{
		public string CustomerJobId { get; private set; }
		public string ImageUrl { get; private set; } = string.Empty;
		public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

		private JobImage() { }

		internal static JobImage Create(string jobId, string imageUrl) =>
			new() { Id = Guid.NewGuid().ToString(), CustomerJobId = jobId, ImageUrl = imageUrl };
	}
}
