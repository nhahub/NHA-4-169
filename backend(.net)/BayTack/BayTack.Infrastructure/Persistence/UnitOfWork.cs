using BayTack.Application.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence
{

	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _context;

		public UnitOfWork(AppDbContext context) => _context = context;

		public Task<int> SaveChangesAsync(CancellationToken ct = default) => _context.SaveChangesAsync(ct);
	}

}
