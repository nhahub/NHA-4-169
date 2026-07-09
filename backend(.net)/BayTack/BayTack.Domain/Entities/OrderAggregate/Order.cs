using BayTack.Domain.Common.BaseEntity;
using BayTack.Domain.Enums;
using BayTack.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Entities.OrderAggregate
{
	public class Order : AuditableEntity<string>
	{
		public string CustomerJobId { get; private set; }
		public string ProviderId { get; private set; }
		public Money FinalPrice { get; private set; } = Money.Zero();
		public DateTime StartDate { get; private set; }
		public DateTime? EndDate { get; private set; }
		public OrderStatus Status { get; private set; } = OrderStatus.Pending;

		private readonly List<OrderStatusHistory> _history = new();
		public IReadOnlyCollection<OrderStatusHistory> History => _history.AsReadOnly();

		private Order() { }

		public static Order Create(string customerJobId, string providerId, Money finalPrice, DateTime startDate, string createdBy)
		{
			var order = new Order
			{
				Id = Guid.NewGuid().ToString(),
				CustomerJobId = customerJobId,
				ProviderId = providerId,
				FinalPrice = finalPrice,
				StartDate = startDate,
				Status = OrderStatus.Pending
			};
			order._history.Add(OrderStatusHistory.Create(order.Id, OrderStatus.Pending, createdBy));
			return order;
		}

		public void ChangeStatus(OrderStatus newStatus, string changedBy)
		{
			if (Status is OrderStatus.Completed or OrderStatus.Cancelled)
				throw new InvalidOperationException($"Cannot change status of a {Status} order.");

			Status = newStatus;
			if (newStatus == OrderStatus.Completed) EndDate = DateTime.UtcNow;
			_history.Add(OrderStatusHistory.Create(Id, newStatus, changedBy));
			SetUpdated(changedBy);
		}
	}

}
