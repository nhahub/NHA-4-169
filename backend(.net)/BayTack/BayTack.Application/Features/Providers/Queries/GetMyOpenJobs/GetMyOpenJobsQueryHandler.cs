using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Providers.Queries.GetMyOpenJobs
{
	public sealed class GetMyOpenJobsQueryHandler : IQueryHandler<GetMyOpenJobsQuery, IReadOnlyList<ProviderJobFeedItemResponse>>
	{
		private readonly IProviderDashboardReadRepository _dashboard;

		public GetMyOpenJobsQueryHandler(IProviderDashboardReadRepository dashboard) => _dashboard = dashboard;

		public async Task<Result<IReadOnlyList<ProviderJobFeedItemResponse>>> Handle(GetMyOpenJobsQuery request, CancellationToken cancellationToken)
		{
			var profile = await _dashboard.GetMyProfileAsync(request.UserId, cancellationToken);
			if (profile is null)
				return Result<IReadOnlyList<ProviderJobFeedItemResponse>>.NotFound("No provider profile found for the current user.");

			var jobs = await _dashboard.GetOpenJobsForCategoryAsync(profile.CategoryId, cancellationToken);
			return Result<IReadOnlyList<ProviderJobFeedItemResponse>>.Success(jobs);
		}
	}
}
