using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Common.Specifications;
using BayTack.Domain.Common.BaseEntity;
using BayTack.Infrastructure.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Repositories
{
	public class RepositoryBase<T, TId> : IRepository<T, TId> where T : BaseEntity<TId>
	{
		protected readonly AppDbContext Context;
		protected readonly DbSet<T> DbSet;

		public RepositoryBase(AppDbContext context)
		{
			Context = context;
			DbSet = context.Set<T>();
		}

		public async Task<T?> GetByIdAsync(TId id, CancellationToken ct = default) =>
			await DbSet.FindAsync(new object?[] { id }, ct);

		public async Task<T?> FirstOrDefaultAsync(Specification<T> spec, CancellationToken ct = default) =>
			await ApplySpec(spec).FirstOrDefaultAsync(ct);

		public async Task<List<T>> ListAsync(Specification<T> spec, CancellationToken ct = default) =>
			await ApplySpec(spec).ToListAsync(ct);

		public async Task<int> CountAsync(Specification<T> spec, CancellationToken ct = default) =>
			await ApplySpec(spec).CountAsync(ct);

		public async Task<bool> AnyAsync(Specification<T> spec, CancellationToken ct = default) =>
			await ApplySpec(spec).AnyAsync(ct);

		public void Add(T entity) => DbSet.Add(entity);
		public void Update(T entity) => DbSet.Update(entity);
		public void Remove(T entity) => DbSet.Remove(entity);

		private IQueryable<T> ApplySpec(Specification<T> spec) =>
			SpecificationEvaluator<T>.GetQuery(DbSet.AsQueryable(), spec);
	}

}
