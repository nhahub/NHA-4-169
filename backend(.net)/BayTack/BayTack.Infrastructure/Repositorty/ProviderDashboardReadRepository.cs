using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Features.Providers.Queries.GetMyOpenJobs;
using BayTack.Application.Features.Providers.Queries.GetMyProviderProfile;
using BayTack.Domain.Entities.JobAggregate;
using BayTack.Domain.Entities.ProviderAggregate;
using BayTack.Domain.Entities.ServiceAggregate;
using BayTack.Domain.Enums;
using BayTack.Infrastructure.Identity;
using BayTack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BayTack.Infrastructure.Repositorty
{
	public sealed class ProviderDashboardReadRepository : IProviderDashboardReadRepository
	{
		private readonly AppDbContext _context;

		public ProviderDashboardReadRepository(AppDbContext context) => _context = context;

		public async Task<MyProviderProfileResponse?> GetMyProfileAsync(string userId, CancellationToken ct)
		{
			// Manual join on UserId - ProviderProfile has no navigation property to AppUser
			// (see ProviderProfileConfiguration), same pattern as ProviderVerificationReadRepository.
			var row = await (
				from profile in _context.Set<ProviderProfile>().AsNoTracking()
				join user in _context.Set<AppUser>().AsNoTracking() on profile.UserId equals user.Id
				where profile.UserId == userId
				select new { profile, user })
				.FirstOrDefaultAsync(ct);

			if (row is null) return null;

			string? categoryName = null;
			if (!string.IsNullOrEmpty(row.profile.PrimaryCategoryId))
			{
				categoryName = await _context.Set<ServiceCategory>().AsNoTracking()
					.Where(c => c.Id == row.profile.PrimaryCategoryId)
					.Select(c => c.Name)
					.FirstOrDefaultAsync(ct);
			}

			var documentsCount = await _context.Set<ProviderProfile>().AsNoTracking()
				.Where(p => p.Id == row.profile.Id)
				.SelectMany(p => p.Documents)
				.CountAsync(ct);

			return new MyProviderProfileResponse(
				row.profile.Id,
				row.user.FullName,
				row.user.PhoneNumber,
				row.profile.ProviderType.ToString(),
				row.profile.VerificationStatus.ToString(),
				row.profile.PrimaryCategoryId,
				categoryName,
				HasSubmittedDocuments: documentsCount > 0,
				row.profile.SuspendReason);
		}

		public async Task<IReadOnlyList<ProviderJobFeedItemResponse>> GetOpenJobsForCategoryAsync(string? categoryId, CancellationToken ct)
		{
			if (string.IsNullOrEmpty(categoryId))
				return Array.Empty<ProviderJobFeedItemResponse>();

			return await (
				from job in _context.Set<CustomerJob>().AsNoTracking()
				join service in _context.Set<Service>().AsNoTracking() on job.ServiceId equals service.Id
				join category in _context.Set<ServiceCategory>().AsNoTracking() on service.CategoryId equals category.Id
				where service.CategoryId == categoryId
					&& (job.Status == JobStatus.Open || job.Status == JobStatus.InBidding)
				orderby job.CreatedAt descending
				select new ProviderJobFeedItemResponse(
					job.Id,
					job.Title,
					job.Description,
					service.CategoryId,
					category.Name,
					job.Location.Details,
					job.CreatedAt))
				.ToListAsync(ct);
		}
	}
}
