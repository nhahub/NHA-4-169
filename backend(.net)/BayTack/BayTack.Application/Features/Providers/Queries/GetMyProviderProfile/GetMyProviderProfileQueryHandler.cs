using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Providers.Queries.GetMyProviderProfile
{
	public sealed class GetMyProviderProfileQueryHandler : IQueryHandler<GetMyProviderProfileQuery, MyProviderProfileResponse>
	{
		private readonly IProviderDashboardReadRepository _dashboard;

		public GetMyProviderProfileQueryHandler(IProviderDashboardReadRepository dashboard) => _dashboard = dashboard;

		public async Task<Result<MyProviderProfileResponse>> Handle(GetMyProviderProfileQuery request, CancellationToken cancellationToken)
		{
			var profile = await _dashboard.GetMyProfileAsync(request.UserId, cancellationToken);
			return profile is null
				? Result<MyProviderProfileResponse>.NotFound("No provider profile found for the current user.")
				: Result<MyProviderProfileResponse>.Success(profile);
		}
	}
}
