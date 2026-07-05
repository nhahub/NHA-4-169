using BayTack.Domain.Common.BaseEntity;
using BayTack.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Entities.ProviderAggregate
{
	public class ProviderDocument : BaseEntity<string>
	{
		public string ProviderProfileId { get; private set; }
		public string DocType { get; private set; } = string.Empty;
		public string DocUrl { get; private set; } = string.Empty;
		public DocumentStatus Status { get; private set; } = DocumentStatus.Pending;
		public DateTime UploadedAt { get; private set; } = DateTime.UtcNow;
		public string? ReviewedBy { get; private set; }
		public DateTime? ReviewedAt { get; private set; }

		private ProviderDocument() { }

		internal static ProviderDocument Create(string providerProfileId, string docType, string docUrl) =>
			new() { ProviderProfileId = providerProfileId, DocType = docType, DocUrl = docUrl };

		public void Approve(string reviewedBy)
		{
			Status = DocumentStatus.Approved;
			ReviewedBy = reviewedBy;
			ReviewedAt = DateTime.UtcNow;
		}

		public void Reject(string reviewedBy)
		{
			Status = DocumentStatus.Rejected;
			ReviewedBy = reviewedBy;
			ReviewedAt = DateTime.UtcNow;
		}
	}
}
