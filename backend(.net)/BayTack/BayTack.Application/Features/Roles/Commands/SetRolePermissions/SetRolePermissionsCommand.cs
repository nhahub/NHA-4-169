using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Roles.Commands.SetRolePermissions
{
	public sealed record SetRolePermissionsCommand(string RoleId, List<string> PermissionIds) : ICommand<RoleResponse>;

	public sealed class SetRolePermissionsCommandHandler : ICommandHandler<SetRolePermissionsCommand, RoleResponse>
	{
		private readonly IRoleRepository _roles;

		public SetRolePermissionsCommandHandler(IRoleRepository roles) => _roles = roles;

		public async Task<Result<RoleResponse>> Handle(SetRolePermissionsCommand request, CancellationToken cancellationToken)
		{
			var (found, role, unknown) = await _roles.SetPermissionsAsync(request.RoleId, request.PermissionIds ?? new(), cancellationToken);

			if (!found) return Result<RoleResponse>.NotFound($"Role '{request.RoleId}' not found");
			if (unknown.Count > 0) return Result<RoleResponse>.Failure($"Unknown permission id(s): {string.Join(", ", unknown)}");

			return Result<RoleResponse>.Success(role!);
		}
	}
}
