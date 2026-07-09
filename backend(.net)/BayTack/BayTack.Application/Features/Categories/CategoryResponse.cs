namespace BayTack.Application.Features.Categories
{
	public sealed record CategoryResponse(
		string Id,
		string Name,
		string Icon,
		string? Description,
		bool IsActive,
		DateTime CreatedAt);
}
