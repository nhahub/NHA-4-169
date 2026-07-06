using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Abstractions.IRepository
{
	public interface IUnitOfWork
	{
		Task<int> SaveChangesAsync(CancellationToken ct = default);
	}

}
