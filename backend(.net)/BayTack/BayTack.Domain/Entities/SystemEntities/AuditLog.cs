using BayTack.Domain.Common.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Entities.SystemEntities
{

	public class AuditLog : BaseEntity<string>
	{
		public string? UserId { get; private set; }
		public string Action { get; private set; } = string.Empty;
		public string TableName { get; private set; } = string.Empty;
		public int? RecordId { get; private set; }
		public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

		private AuditLog() { }

		public static AuditLog Create(string? userId, string action, string tableName, int? recordId) =>
			new() { UserId = userId, Action = action, TableName = tableName, RecordId = recordId };
	}

}
