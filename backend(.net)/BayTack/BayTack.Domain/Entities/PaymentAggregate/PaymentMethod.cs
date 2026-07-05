using BayTack.Domain.Common.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Entities.PaymentAggregate
{

	public class PaymentMethod : BaseEntity<string>
	{
		public string Name { get; private set; } = string.Empty;
		public string? Description { get; private set; }

		private PaymentMethod() { }

		public static PaymentMethod Create(string name, string? description = null) =>
			new() { Name = name, Description = description };
	}
}
