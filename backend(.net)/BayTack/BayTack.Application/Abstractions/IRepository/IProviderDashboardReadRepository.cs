using BayTack.Application.Features.Providers.Queries.GetMyOpenJobs;
using BayTack.Application.Features.Providers.Queries.GetMyProviderProfile;

namespace BayTack.Application.Abstractions.IRepository
{
	/// <summary>
	/// Read-only queries backing the provider's own dashboard (Front_end/provider/dashboard).
	/// Kept separate from IProviderVerificationReadRepository because that one is admin-facing
	/// (verification queue) while this one is scoped to "the currently signed-in provider".
	/// </summary>
	public interface IProviderDashboardReadRepository
	{
		/// <summary>Null when the signed-in user has no provider profile yet (hasn't completed onboarding).</summary>
		Task<MyProviderProfileResponse?> GetMyProfileAsync(string userId, CancellationToken ct);

		/// <summary>Open customer job requests whose Service belongs to the given category.
		/// Pass a null/empty categoryId to get an empty list (a provider with no category set
		/// yet has nothing to match against).</summary>
		Task<IReadOnlyList<ProviderJobFeedItemResponse>> GetOpenJobsForCategoryAsync(string? categoryId, CancellationToken ct);
	}
}
