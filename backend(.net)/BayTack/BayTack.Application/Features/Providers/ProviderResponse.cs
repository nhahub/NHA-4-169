namespace BayTack.Application.Features.Providers
{
	public sealed record ProviderResponse(
		string Id,
		string Name,
		string? Email,
		string? CategoryId,
		string? CategoryName,
		string ProviderType,
		int YearsOfExperience,
		string Status,
		string? SuspendReason,
		DateTime CreatedAt);
}
