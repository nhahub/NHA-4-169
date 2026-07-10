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
		public bool IsActive { get; private set; }

		private readonly List<Service> _services = new();

		public IReadOnlyCollection<Service> Services => _services.AsReadOnly();

		public string? Icon { get; set; }

		private ServiceCategory() { }

		public static ServiceCategory Create(string name, string? icon, string? description = null) =>
			new() { Id = Guid.NewGuid().ToString(), Name = name, Description = description };


		public void UpdateDetails(string? name,string? icon, string? description, bool isActive, string? updatedBy)
		{
			Name = name ?? Name;
			Icon = icon ?? Icon;
			Description = description ?? Description;
			IsActive = isActive;
			UpdatedBy = updatedBy ?? UpdatedBy;
			UpdatedAt = DateTime.UtcNow;
		}

		public void ToggleActive(string? updatedBy)
		{
			IsActive = !IsActive;
			UpdatedBy = updatedBy ?? UpdatedBy;
			UpdatedAt = DateTime.UtcNow;
		} 
	}
}
