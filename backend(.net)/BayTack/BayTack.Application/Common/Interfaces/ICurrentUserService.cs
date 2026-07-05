using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Common.Interfaces
{
	public interface ICurrentUserService
	{
		int? UserId { get; }
		string? Email { get; }
		bool IsInRole(string role);
		bool IsAuthenticated { get; }
	}
}
