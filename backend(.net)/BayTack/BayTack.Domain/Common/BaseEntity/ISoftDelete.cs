using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Domain.Common.BaseEntity
{

	public interface ISoftDelete
	{
		bool IsDeleted { get; }
		DateTime? DeletedAt { get; }
		string? DeletedBy { get; }
		string? DeleteReason { get; }
	}
}
