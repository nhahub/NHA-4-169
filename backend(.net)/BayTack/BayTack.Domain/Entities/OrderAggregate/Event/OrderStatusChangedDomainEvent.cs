using BayTack.Domain.Common.Events;
using BayTack.Domain.Enums;

namespace BayTack.Domain.Entities.OrderAggregate.Event
{
	public sealed class OrderStatusChangedDomainEvent : IDomainEvent
	{
		public string OrderId { get; }
		public OrderStatus NewStatus { get; }
		public string ChangedBy { get; }
		public DateTime OccurredOn { get; }

		public OrderStatusChangedDomainEvent(string orderId, OrderStatus newStatus, string changedBy)
		{
			OrderId = orderId;
			NewStatus = newStatus;
			ChangedBy = changedBy;
			OccurredOn = DateTime.UtcNow;
		}
	}
}
