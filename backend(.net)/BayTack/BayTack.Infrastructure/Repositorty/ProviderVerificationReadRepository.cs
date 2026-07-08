using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Features.Verification.Commands.MarkUnderReview;
using BayTack.Domain.Entities.ProviderAggregate;
using BayTack.Domain.Enums;
using BayTack.Infrastructure.Identity;
using BayTack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BayTack.Infrastructure.Repositorty
{
	namespace BayTack.Infrastructure.Repositorty
	{
		public sealed class ProviderVerificationReadRepository : IProviderVerificationReadRepository
		{
			private readonly AppDbContext _context;

			public ProviderVerificationReadRepository(AppDbContext context) => _context = context;

			public async Task<IReadOnlyList<VerificationEntryResponse>> GetQueueAsync(
				string? status, CancellationToken ct)
			{
				var query = BaseQuery();

				if (!string.IsNullOrWhiteSpace(status) &&
					Enum.TryParse<VerificationStatus>(status, ignoreCase: true, out var parsedStatus))
				{
					query = query.Where(x => x.Profile.VerificationStatus == parsedStatus);
				}

				return await query
					.OrderBy(x => x.Profile.CreatedAt)
					.Select(x => new VerificationEntryResponse(
						x.Profile.Id,
						x.User.FullName,
						x.Profile.ProviderType.ToString(), // TEMP placeholder for Category — see note
						x.Profile.ProviderType.ToString(),
						x.Profile.VerificationStatus.ToString(),
						x.Profile.CreatedAt,
						x.Profile.Documents.Select(d => d.DocUrl).FirstOrDefault()))
					.ToListAsync(ct);
			}

			public async Task<VerificationDetailResponse?> GetDetailAsync(string providerProfileId, CancellationToken ct)
			{
				var result = await BaseQuery()
					.Where(x => x.Profile.Id == providerProfileId)
					.Select(x => new
					{
						x.Profile,
						x.User.FullName
					})
					.FirstOrDefaultAsync(ct);

				if (result is null) return null;

				var summary = new VerificationEntryResponse(
					result.Profile.Id,
					result.FullName,
					result.Profile.ProviderType.ToString(), // TEMP placeholder for Category
					result.Profile.ProviderType.ToString(),
					result.Profile.VerificationStatus.ToString(),
					result.Profile.CreatedAt,
					result.Profile.Documents.Select(d => d.DocUrl).FirstOrDefault());

				var documents = result.Profile.Documents.Select(d =>
					new VerificationDocumentResponse(d.Id, d.DocType.ToString(), d.DocUrl, d.Status.ToString())).ToList();

				return new VerificationDetailResponse(summary, documents);
			}

			// Manual join on UserId — ProviderProfile has no navigation property to AppUser.
			private IQueryable<(ProviderProfile Profile, AppUser User)> BaseQuery() =>
				from profile in _context.Set<ProviderProfile>()
				.AsNoTracking()
				join user in _context.Set<AppUser>()
				.AsNoTracking()
					on profile.UserId equals user.Id
				select new ValueTuple<ProviderProfile, AppUser>(profile, user);
		}
	}
}
