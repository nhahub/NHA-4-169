using BayTack.Domain.Common.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Entities.Location
{
	public class City : BaseEntity<string>
	{
		public string Name { get; private set; } = string.Empty;

		private readonly List<Area> _areas = new();
		public IReadOnlyCollection<Area> Areas => _areas.AsReadOnly();

		private City() { }

		public static City Create(string name) => new() { Id = Guid.NewGuid().ToString(), Name = name };

		public Area AddArea(string name)
		{
			var area = Area.Create(Id, name);
			_areas.Add(area);
			return area;
		}
	}
}
