using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Common.BaseEntity
{
	public abstract class AuditableEntity<TId> : BaseEntity<TId>
	{
		public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; protected set; }
		public string? UpdatedBy { get; protected set; }

		protected void SetUpdated(string? updatedBy)
		{
			UpdatedAt = DateTime.UtcNow;
			UpdatedBy = updatedBy;
		}
	}
}
