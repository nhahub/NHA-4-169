using BayTack.Domain.Common.BaseEntity;
using BayTack.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Entities.OrderAggregate
{

	public class OrderStatusHistory : BaseEntity<string>
	{
		public string OrderId { get; private set; }
		public OrderStatus Status { get; private set; }
		public string ChangedBy { get; private set; }
		public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

		private OrderStatusHistory() { }

		internal static OrderStatusHistory Create(string orderId, OrderStatus status, string changedBy) =>
			new() { OrderId = orderId, Status = status, ChangedBy = changedBy };
	}
}
