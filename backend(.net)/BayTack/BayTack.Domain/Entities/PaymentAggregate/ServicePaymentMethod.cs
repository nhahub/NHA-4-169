using BayTack.Domain.Common.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Entities.PaymentAggregate
{

	public class ServicePaymentMethod : BaseEntity<string>
	{
		public string ServiceId { get; private set; }
		public string PaymentMethodId { get; private set; }
		public bool Allowed { get; private set; } = true;

		private ServicePaymentMethod() { }

		internal static ServicePaymentMethod Create(string serviceId, string paymentMethodId) =>
			new() { ServiceId = serviceId, PaymentMethodId = paymentMethodId, Allowed = true };
	}
}
