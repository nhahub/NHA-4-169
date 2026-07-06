using BayTack.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.ValueObjects
{
	public sealed class Address : ValueObject
	{
		public string Details { get; private set; }
		public int CityId { get; private set; }
		public int? AreaId { get; private set; }

		private Address() { Details = string.Empty; }

		private Address(string details, int cityId, int? areaId)
		{
			if (string.IsNullOrWhiteSpace(details))
				throw new ArgumentException("Address details are required.", nameof(details));
			Details = details;
			CityId = cityId;
			AreaId = areaId;
		}

		public static Address Create(string details, int cityId, int? areaId = null) => new(details, cityId, areaId);

		protected override IEnumerable<object?> GetEqualityComponents()
		{
			yield return Details;
			yield return CityId;
			yield return AreaId;
		}
	}

}
