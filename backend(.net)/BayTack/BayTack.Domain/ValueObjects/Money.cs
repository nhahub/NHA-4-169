using BayTack.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.ValueObjects
{
	public sealed class Money : ValueObject
	{
		public decimal Amount { get; private set; }
		public string Currency { get; private set; }

		private Money() { Currency = "EGP"; }

		private Money(decimal amount, string currency)
		{
			if (amount < 0) throw new ArgumentException("Amount cannot be negative.", nameof(amount));
			if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency is required.", nameof(currency));
			Amount = decimal.Round(amount, 2);
			Currency = currency;
		}

		public static Money Create(decimal amount, string currency = "EGP") => new(amount, currency);
		public static Money Zero(string currency = "EGP") => new(0, currency);

		public Money Add(Money other)
		{
			EnsureSameCurrency(other);
			return new Money(Amount + other.Amount, Currency);
		}

		public Money Subtract(Money other)
		{
			EnsureSameCurrency(other);
			var result = Amount - other.Amount;
			if (result < 0) throw new InvalidOperationException("Resulting amount cannot be negative.");
			return new Money(result, Currency);
		}

		private void EnsureSameCurrency(Money other)
		{
			if (Currency != other.Currency)
				throw new InvalidOperationException("Cannot operate on Money values with different currencies.");
		}

		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return Amount;
			yield return Currency;
		}

		public override string ToString() => $"{Amount:N2} {Currency}";
	}
}
