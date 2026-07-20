using BayTack.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.ValueObjects
{
	public sealed class Address : ValueObject
	{
		public string Details { get; private set; }
		public string CityId { get; private set; }
		public string? AreaId { get; private set; }

		private Address() { Details = string.Empty; CityId = string.Empty; }

		private Address(string details, string cityId, string? areaId)
		{
			if (string.IsNullOrWhiteSpace(details))
				throw new ArgumentException("Address details are required.", nameof(details));
			if (string.IsNullOrWhiteSpace(cityId))
				throw new ArgumentException("A city is required.", nameof(cityId));
			Details = details;
			CityId = cityId;
			AreaId = areaId;
		}

		public static Address Create(string details, string cityId, string? areaId = null) => new(details, cityId, areaId);

		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return Details;
			yield return CityId;
			yield return AreaId;
		}
	}

}
