using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Roles.Queries.GetAllRoles
{
	public sealed record GetAllRolesQuery : IQuery<List<RoleResponse>>;

	public sealed class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, List<RoleResponse>>
	{
		private readonly IRoleRepository _roles;

		public GetAllRolesQueryHandler(IRoleRepository roles) => _roles = roles;

		public async Task<Result<List<RoleResponse>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken) =>
			Result<List<RoleResponse>>.Success(await _roles.GetAllAsync(cancellationToken));
	}
}
