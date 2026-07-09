using BayTack.Application.Features.Roles;

namespace BayTack.Application.Abstractions.IRepository
{
	/// <summary>
	/// Roles are ASP.NET Identity roles (IdentityRole&lt;string&gt;), and permissions are
	/// modeled as role claims (claim type "permission") rather than a custom Domain
	/// entity — there's no dedicated Role/Permission aggregate in BayTack.Domain, so
	/// this repository wraps RoleManager/UserManager directly, the same way
	/// IUserRepository wraps UserManager for the Users feature.
	/// </summary>
	public interface IRoleRepository
	{
		Task<List<RoleResponse>> GetAllAsync(CancellationToken cancellationToken);
		Task<RoleResponse?> GetByIdAsync(string id, CancellationToken cancellationToken);
		/// <summary>Returns null if a role with the same name already exists.</summary>
		Task<RoleResponse?> CreateAsync(string name, List<string>? permissionIds, CancellationToken cancellationToken);
		Task<RoleResponse?> RenameAsync(string id, string name, CancellationToken cancellationToken);
		Task<(bool Success, string? Error)> DeleteAsync(string id, CancellationToken cancellationToken);
		/// <summary>Found is false if roleId doesn't exist. UnknownPermissionIds is non-empty (and nothing is changed) if any id isn't a real permission.</summary>
		Task<(bool Found, RoleResponse? Role, List<string> UnknownPermissionIds)> SetPermissionsAsync(string roleId, List<string> permissionIds, CancellationToken cancellationToken);
		List<PermissionResponse> GetPermissionCatalog();
	}
}
