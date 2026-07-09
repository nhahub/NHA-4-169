namespace BayTack.Application.Features.Roles
{
	public sealed record RoleResponse(string Id, string Name, IReadOnlyList<string> Permissions, int UserCount);

	public sealed record PermissionResponse(string Id, string Label, string Group);
}
