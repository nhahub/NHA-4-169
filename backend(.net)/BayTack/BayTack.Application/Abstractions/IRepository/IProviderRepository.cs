using BayTack.Application.Features.Providers;

namespace BayTack.Application.Abstractions.IRepository
{
	public interface IProviderRepository
	{
		Task<(List<ProviderResponse> Items, int Total)> GetAllAsync(string? search, string? categoryId, string? status, int page, int limit, CancellationToken cancellationToken);
		Task<ProviderResponse?> GetByIdAsync(string id, CancellationToken cancellationToken);
		Task<(int Total, int Verified, int Pending, int Suspended)> GetStatsAsync(CancellationToken cancellationToken);
		Task<List<ProviderResponse>> GetRecentAsync(int limit, CancellationToken cancellationToken);
		Task<ProviderResponse?> UpdateAsync(string id, string? providerType, int? yearsOfExperience, string? categoryId, CancellationToken cancellationToken);
		Task<ProviderResponse?> ApproveAsync(string id, CancellationToken cancellationToken);
		Task<ProviderResponse?> SuspendAsync(string id, string? reason, CancellationToken cancellationToken);
		Task<ProviderResponse?> ReinstateAsync(string id, CancellationToken cancellationToken);
		Task<ProviderResponse?> MarkUnderReviewAsync(string id, CancellationToken cancellationToken);

		/// <summary>Verification queue: providers whose Status is Pending or UnderReview. status filters further within that set.</summary>
		Task<List<ProviderResponse>> GetVerificationQueueAsync(string? status, CancellationToken cancellationToken);
	}
}
