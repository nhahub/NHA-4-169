using BayTack.Domain.Common.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Entities.ProviderAggregate
{
	public class ProviderPortfolioItem : BaseEntity<string>
	{
		public string ProviderProfileId { get; private set; }
		public string Title { get; private set; } = string.Empty;
		public string? Description { get; private set; }
		public string? ImageUrl { get; private set; }
		public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

		private ProviderPortfolioItem() { }

		internal static ProviderPortfolioItem Create(string providerProfileId, string title, string? description, string? imageUrl) =>
			new() { Id = Guid.NewGuid().ToString(), ProviderProfileId = providerProfileId, Title = title, Description = description, ImageUrl = imageUrl };
	}
}
