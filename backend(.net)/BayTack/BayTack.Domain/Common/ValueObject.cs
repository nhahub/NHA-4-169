using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Common
{
	public abstract class ValueObject
	{
		protected abstract IEnumerable<object?> GetEqualityComponents();

		public override bool Equals(object? obj)
		{
			if (obj is null || obj.GetType() != GetType()) return false;
			var other = (ValueObject)obj;
			return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
		}

		public override int GetHashCode() =>
			GetEqualityComponents().Aggregate(1, (current, obj) => current * 23 + (obj?.GetHashCode() ?? 0));

		public static bool operator ==(ValueObject? a, ValueObject? b)
		{
			if (a is null && b is null) return true;
			if (a is null || b is null) return false;
			return a.Equals(b);
		}

		public static bool operator !=(ValueObject? a, ValueObject? b) => !(a == b);
	}

}
