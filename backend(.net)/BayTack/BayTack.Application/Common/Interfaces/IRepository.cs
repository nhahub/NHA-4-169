using BayTack.Domain.Common.BaseEntity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Common.Interfaces
{
	public interface IRepository<T, TId> where T : BaseEntity<TId>
	{
		Task<T?> GetByIdAsync(TId id, CancellationToken ct = default);
		Task<T?> FirstOrDefaultAsync(Specification<T> spec, CancellationToken ct = default);
		Task<List<T>> ListAsync(Specification<T> spec, CancellationToken ct = default);
		Task<int> CountAsync(Specification<T> spec, CancellationToken ct = default);
		Task<bool> AnyAsync(Specification<T> spec, CancellationToken ct = default);

		void Add(T entity);
		void Update(T entity);
		void Remove(T entity);
	}

}
