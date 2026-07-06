using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Common.BaseEntity
{
	public abstract class SoftDeletableEntity<TId> : AuditableEntity<TId>, ISoftDelete
	{
		public bool IsDeleted { get; private set; }
		public DateTime? DeletedAt { get; private set; }
		public string? DeletedBy { get; private set; }
		public string? DeleteReason { get; private set; }

		public void Delete(string? deletedBy, string? reason = null)
		{
			if (IsDeleted) return;
			IsDeleted = true;
			DeletedAt = DateTime.UtcNow;
			DeletedBy = deletedBy;
			DeleteReason = reason;
		}

		public void Restore()
		{
			IsDeleted = false;
			DeletedAt = null;
			DeletedBy = null;
			DeleteReason = null;
		}
	}

}
