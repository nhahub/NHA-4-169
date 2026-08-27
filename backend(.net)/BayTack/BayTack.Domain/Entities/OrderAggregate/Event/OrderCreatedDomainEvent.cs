using BayTack.Domain.Common.Events;
using BayTack.Domain.ValueObjects;

namespace BayTack.Domain.Entities.OrderAggregate.Event
{
	public sealed class OrderCreatedDomainEvent : IDomainEvent
	{
		public string OrderId { get; }
		public string CustomerJobId { get; }
		public string ProviderId { get; }
		public Money FinalPrice { get; }
		public DateTime StartDate { get; }
		public DateTime OccurredOn { get; }

		public OrderCreatedDomainEvent(string orderId, string customerJobId, string providerId, Money finalPrice, DateTime startDate)
		{
			OrderId = orderId;
			CustomerJobId = customerJobId;
			ProviderId = providerId;
			FinalPrice = finalPrice;
			StartDate = startDate;
			OccurredOn = DateTime.UtcNow;
		}
	}
}
