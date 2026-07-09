using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Roles.Queries.GetPermissions
{
	public sealed record GetPermissionsQuery : IQuery<List<PermissionResponse>>;

	public sealed class GetPermissionsQueryHandler : IQueryHandler<GetPermissionsQuery, List<PermissionResponse>>
	{
		private readonly IRoleRepository _roles;

		public GetPermissionsQueryHandler(IRoleRepository roles) => _roles = roles;

		public Task<Result<List<PermissionResponse>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken) =>
			Task.FromResult(Result<List<PermissionResponse>>.Success(_roles.GetPermissionCatalog()));
	}
}
