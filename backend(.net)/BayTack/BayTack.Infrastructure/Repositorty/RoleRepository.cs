using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Features.Roles;
using BayTack.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace BayTack.Infrastructure.Repositorty
{
	public sealed class RoleRepository : IRoleRepository
	{
		private const string PermissionClaimType = "permission";

		// Static catalog of grantable permissions — there's no Permission table in the
		// Domain model, so this mirrors what the admin Roles & Permissions page needs.
		private static readonly List<PermissionResponse> Catalog = new()
		{
			new("users.read",        "View Users",        "Users"),
			new("users.manage",      "Manage Users",      "Users"),
			new("orders.read",       "View Orders",       "Orders"),
			new("orders.manage",     "Manage Orders",     "Orders"),
			new("providers.manage",  "Manage Providers",  "Providers"),
			new("analytics.read",    "View Analytics",    "Analytics"),
			new("payouts.manage",    "Manage Payouts",    "Finance"),
			new("categories.manage", "Manage Categories", "Content"),
			new("roles.manage",      "Manage Roles",      "Admin"),
			new("all",               "Full Access",       "Admin"),
		};

		private readonly RoleManager<IdentityRole<string>> _roleManager;
		private readonly UserManager<AppUser> _userManager;

		public RoleRepository(RoleManager<IdentityRole<string>> roleManager, UserManager<AppUser> userManager)
		{
			_roleManager = roleManager;
			_userManager = userManager;
		}

		public async Task<List<RoleResponse>> GetAllAsync(CancellationToken cancellationToken)
		{
			var roles = _roleManager.Roles.ToList();
			var result = new List<RoleResponse>();

			foreach (var role in roles)
				result.Add(await MapAsync(role));

			return result;
		}

		public async Task<RoleResponse?> GetByIdAsync(string id, CancellationToken cancellationToken)
		{
			var role = await _roleManager.FindByIdAsync(id);
			return role is null ? null : await MapAsync(role);
		}

		public async Task<RoleResponse?> CreateAsync(string name, List<string>? permissionIds, CancellationToken cancellationToken)
		{
			if (await _roleManager.RoleExistsAsync(name)) return null;

			var role = new IdentityRole<string>(name) { Id = Guid.NewGuid().ToString() };
			var createResult = await _roleManager.CreateAsync(role);
			if (!createResult.Succeeded) return null;

			var validIds = Catalog.Select(p => p.Id).ToHashSet();
			foreach (var permissionId in (permissionIds ?? new()).Where(validIds.Contains))
				await _roleManager.AddClaimAsync(role, new System.Security.Claims.Claim(PermissionClaimType, permissionId));

			return await MapAsync(role);
		}

		public async Task<RoleResponse?> RenameAsync(string id, string name, CancellationToken cancellationToken)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role is null) return null;

			role.Name = name;
			var updateResult = await _roleManager.UpdateAsync(role);
			return updateResult.Succeeded ? await MapAsync(role) : null;
		}

		public async Task<(bool Success, string? Error)> DeleteAsync(string id, CancellationToken cancellationToken)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role is null) return (false, "Role not found");

			if (string.Equals(role.Name, "Super Admin", StringComparison.OrdinalIgnoreCase))
				return (false, "The Super Admin role cannot be deleted");

			var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
			if (usersInRole.Count > 0)
				return (false, $"Cannot delete a role with {usersInRole.Count} assigned user(s)");

			var deleteResult = await _roleManager.DeleteAsync(role);
			return deleteResult.Succeeded ? (true, null) : (false, "Failed to delete role");
		}

		public async Task<(bool Found, RoleResponse? Role, List<string> UnknownPermissionIds)> SetPermissionsAsync(string roleId, List<string> permissionIds, CancellationToken cancellationToken)
		{
			var role = await _roleManager.FindByIdAsync(roleId);
			if (role is null) return (false, null, new List<string>());

			var validIds = Catalog.Select(p => p.Id).ToHashSet();
			var unknown = permissionIds.Where(p => !validIds.Contains(p)).ToList();
			if (unknown.Count > 0) return (true, await MapAsync(role), unknown);

			var existingClaims = (await _roleManager.GetClaimsAsync(role)).Where(c => c.Type == PermissionClaimType).ToList();
			foreach (var claim in existingClaims)
				await _roleManager.RemoveClaimAsync(role, claim);

			foreach (var permissionId in permissionIds)
				await _roleManager.AddClaimAsync(role, new System.Security.Claims.Claim(PermissionClaimType, permissionId));

			return (true, await MapAsync(role), new List<string>());
		}

		public List<PermissionResponse> GetPermissionCatalog() => Catalog.ToList();

		private async Task<RoleResponse> MapAsync(IdentityRole<string> role)
		{
			var claims = await _roleManager.GetClaimsAsync(role);
			var permissions = claims.Where(c => c.Type == PermissionClaimType).Select(c => c.Value).ToList();
			var userCount = (await _userManager.GetUsersInRoleAsync(role.Name!)).Count;

			return new RoleResponse(role.Id, role.Name!, permissions, userCount);
		}
	}
}
