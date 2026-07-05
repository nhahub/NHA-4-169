using BayTack.Domain.Common.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Entities.ServiceAggregate
{

	public class ServiceCategory : AuditableEntity<string>
	{
		public string Name { get; private set; } = string.Empty;
		public string? Description { get; private set; }

		private readonly List<Service> _services = new();
		public IReadOnlyCollection<Service> Services => _services.AsReadOnly();

		private ServiceCategory() { }

		public static ServiceCategory Create(string name, string? description = null) =>
			new() { Name = name, Description = description };
	}
}
