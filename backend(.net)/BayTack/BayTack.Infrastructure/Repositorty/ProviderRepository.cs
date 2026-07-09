using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Features.Providers;
using BayTack.Domain.Entities.ProviderAggregate;
using BayTack.Domain.Enums;
using BayTack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BayTack.Infrastructure.Repositorty
{
	public sealed class ProviderRepository : IProviderRepository
	{
		private readonly AppDbContext _context;

		public ProviderRepository(AppDbContext context) => _context = context;

		// Raw, EF-translatable projection (no enum .ToString() yet) — filtering happens
		// on this shape, then Map() converts to the string-based ProviderResponse DTO
		// after the data is already in memory.
		private sealed record Row(string Id, string Name, string? Email, string? CategoryId, string? CategoryName,
			ProviderType ProviderType, int YearsOfExperience, VerificationStatus Status, string? SuspendReason, DateTime CreatedAt);

		private static ProviderResponse Map(Row r) => new(
			r.Id, r.Name, r.Email, r.CategoryId, r.CategoryName,
			r.ProviderType.ToString(), r.YearsOfExperience, r.Status.ToString(), r.SuspendReason, r.CreatedAt);

		private IQueryable<Row> BaseQuery() =>
			from p in _context.ProviderProfiles.AsNoTracking()
			join u in _context.Users.AsNoTracking() on p.UserId equals u.Id
			join c in _context.ServiceCategories.AsNoTracking() on p.PrimaryCategoryId equals c.Id into categoryJoin
			from category in categoryJoin.DefaultIfEmpty()
			select new Row(p.Id, u.FullName, u.Email, p.PrimaryCategoryId, category != null ? category.Name : null,
				p.ProviderType, p.YearsOfExperience, p.VerificationStatus, p.SuspendReason, p.CreatedAt);

		public async Task<(List<ProviderResponse> Items, int Total)> GetAllAsync(string? search, string? categoryId, string? status, int page, int limit, CancellationToken cancellationToken)
		{
			var query = BaseQuery();

			if (!string.IsNullOrWhiteSpace(categoryId))
				query = query.Where(p => p.CategoryId == categoryId);

			if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<VerificationStatus>(status, true, out var parsedStatus))
				query = query.Where(p => p.Status == parsedStatus);

			if (!string.IsNullOrWhiteSpace(search))
			{
				var s = search.Trim().ToLower();
				query = query.Where(p => p.Name.ToLower().Contains(s) || (p.Email != null && p.Email.ToLower().Contains(s)));
			}

			var total = await query.CountAsync(cancellationToken);
			var rows = await query
				.OrderByDescending(p => p.CreatedAt)
				.Skip((Math.Max(page, 1) - 1) * Math.Max(limit, 1))
				.Take(Math.Max(limit, 1))
				.ToListAsync(cancellationToken);

			return (rows.Select(Map).ToList(), total);
		}

		public async Task<ProviderResponse?> GetByIdAsync(string id, CancellationToken cancellationToken)
		{
			var row = await BaseQuery().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
			return row is null ? null : Map(row);
		}

		public async Task<(int Total, int Verified, int Pending, int Suspended)> GetStatsAsync(CancellationToken cancellationToken)
		{
			var statuses = await _context.ProviderProfiles.AsNoTracking()
				.Select(p => p.VerificationStatus)
				.ToListAsync(cancellationToken);

			return (
				Total: statuses.Count,
				Verified: statuses.Count(s => s == VerificationStatus.Verified),
				Pending: statuses.Count(s => s is VerificationStatus.Pending or VerificationStatus.UnderReview),
				Suspended: statuses.Count(s => s == VerificationStatus.Suspended)
			);
		}

		public async Task<List<ProviderResponse>> GetRecentAsync(int limit, CancellationToken cancellationToken)
		{
			var rows = await BaseQuery().OrderByDescending(p => p.CreatedAt).Take(Math.Max(limit, 1)).ToListAsync(cancellationToken);
			return rows.Select(Map).ToList();
		}

		public async Task<ProviderResponse?> UpdateAsync(string id, string? providerType, int? yearsOfExperience, string? categoryId, CancellationToken cancellationToken)
		{
			var provider = await _context.ProviderProfiles.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
			if (provider is null) return null;

			ProviderType? parsedType = providerType is not null && Enum.TryParse<ProviderType>(providerType, true, out var t) ? t : null;
			provider.UpdateDetails(parsedType, yearsOfExperience, categoryId, updatedBy: null);

			await _context.SaveChangesAsync(cancellationToken);
			return await GetByIdAsync(id, cancellationToken);
		}

		public Task<ProviderResponse?> ApproveAsync(string id, CancellationToken cancellationToken) =>
			Transition(id, p => p.AdminApprove(null), cancellationToken);

		public Task<ProviderResponse?> SuspendAsync(string id, string? reason, CancellationToken cancellationToken) =>
			Transition(id, p => p.Suspend(reason, null), cancellationToken);

		public Task<ProviderResponse?> ReinstateAsync(string id, CancellationToken cancellationToken) =>
			Transition(id, p => p.Reinstate(null), cancellationToken);

		public Task<ProviderResponse?> MarkUnderReviewAsync(string id, CancellationToken cancellationToken) =>
			Transition(id, p => p.MarkUnderReview(null), cancellationToken);

		public async Task<List<ProviderResponse>> GetVerificationQueueAsync(string? status, CancellationToken cancellationToken)
		{
			var query = BaseQuery().Where(p => p.Status == VerificationStatus.Pending || p.Status == VerificationStatus.UnderReview);

			if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<VerificationStatus>(status, true, out var parsedStatus))
				query = query.Where(p => p.Status == parsedStatus);

			var rows = await query.OrderBy(p => p.CreatedAt).ToListAsync(cancellationToken);
			return rows.Select(Map).ToList();
		}

		private async Task<ProviderResponse?> Transition(string id, Action<ProviderProfile> transition, CancellationToken cancellationToken)
		{
			var provider = await _context.ProviderProfiles.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
			if (provider is null) return null;

			transition(provider);
			await _context.SaveChangesAsync(cancellationToken);
			return await GetByIdAsync(id, cancellationToken);
		}
	}
}
