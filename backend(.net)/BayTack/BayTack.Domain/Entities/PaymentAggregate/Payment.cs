using BayTack.Domain.Common.BaseEntity;
using BayTack.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Entities.PaymentAggregate
{
	public class Payment : BaseEntity<string>
	{
		public string OrderId { get; private set; }
		public int MethodId { get; private set; }
		public Money Amount { get; private set; } = Money.Zero();
		public Money OurCommission { get; private set; } = Money.Zero();
		public Money ProviderReceived { get; private set; } = Money.Zero();
		public int AdminId { get; private set; }
		public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

		private Payment() { }

		public static Payment Create(string orderId, int methodId, Money amount, Money commission, int adminId)
		{
			if (commission.Amount > amount.Amount)
				throw new ArgumentException("Commission cannot exceed the total payment amount.");

			return new Payment
			{
				OrderId = orderId,
				MethodId = methodId,
				Amount = amount,
				OurCommission = commission,
				ProviderReceived = amount.Subtract(commission),
				AdminId = adminId
			};
		}
	}

}
