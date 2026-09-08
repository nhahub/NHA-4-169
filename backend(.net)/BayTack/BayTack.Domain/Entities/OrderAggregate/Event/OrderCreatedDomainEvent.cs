using BayTack.Domain.Common.Events;
using BayTack.Domain.ValueObjects;

namespace BayTack.Domain.Entities.OrderAggregate.Event
{
	public sealed class OrderCreatedDomainEvent : IDomainEvent
	{
		public string OrderId { get; }
		public string CustomerId { get; }
		public string CustomerJobId { get; }
		public string ServiceId { get; }
		public string Title { get; }
		public string Description { get; }
		public string ProviderId { get; }
		public string ProviderName { get; }
		public Money FinalPrice { get; }
		public DateTime StartDate { get; }
		public DateTime OccurredOn { get; }

		public OrderCreatedDomainEvent(
			string orderId, string customerId, string customerJobId, string serviceId,
			string title, string description, string providerId, string providerName,
			Money finalPrice, DateTime startDate)
		{
			OrderId = orderId;
			CustomerId = customerId;
			CustomerJobId = customerJobId;
			ServiceId = serviceId;
			Title = title;
			Description = description;
			ProviderId = providerId;
			ProviderName = providerName;
			FinalPrice = finalPrice;
			StartDate = startDate;
			OccurredOn = DateTime.UtcNow;
		}
	}
}
