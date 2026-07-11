using BayTack.Domain.Common.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;
namespace BayTack.Domain.Entities
{
	public class Review : BaseEntity<string>
	{
		public string OrderId { get; private set; }
		public string CustomerId { get; private set; }
		public int Rating { get; private set; }
		public string? ReviewText { get; private set; }
		public string? ProviderResponse { get; private set; }
		public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
		private Review() { }
		public static Review Create(string orderId, string customerId, int rating, string? reviewText)
		{
			if (rating is < 1 or > 5)
				throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");
			return new Review
			{
				Id = Guid.NewGuid().ToString(),
				OrderId = orderId,
				CustomerId = customerId,
				Rating = rating,
				ReviewText = reviewText
			};
		}

		public void Respond(string responseText)
		{
			if (string.IsNullOrWhiteSpace(responseText))
				throw new ArgumentException("Response text is required.", nameof(responseText));
			ProviderResponse = responseText;
		}
	}
}
