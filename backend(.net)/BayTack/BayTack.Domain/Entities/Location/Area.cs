using BayTack.Domain.Common.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Entities.Location
{
	public class Area : BaseEntity<string>
	{
		public string CityId { get; private set; }
		public string Name { get; private set; } = string.Empty;

		private Area() { }

		internal static Area Create(string cityId, string name) => new() { CityId = cityId, Name = name };
	}
}
