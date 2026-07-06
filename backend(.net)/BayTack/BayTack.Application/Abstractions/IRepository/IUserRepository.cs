using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Abstractions.IRepository
{
	public interface IUserRepository
	{
		Task<(IReadOnlyList<UserResponse> Items, int TotalCount)> SearchAsync(
			string? search,
			string? role,
			int page,
			int limit,
			CancellationToken cancellationToken);
	}
}
