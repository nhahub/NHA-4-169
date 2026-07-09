using BayTack.Domain.Common.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Entities.SystemEntities
{

	public class Attachment : BaseEntity<string>
	{
		public string FileUrl { get; private set; } = string.Empty;
		public string FileType { get; private set; } = string.Empty;
		public string UploadedBy { get; private set; }
		public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

		private Attachment() { }

		public static Attachment Create(string fileUrl, string fileType, string uploadedBy) =>
			new() { Id = Guid.NewGuid().ToString(), FileUrl = fileUrl, FileType = fileType, UploadedBy = uploadedBy };
	}
}
