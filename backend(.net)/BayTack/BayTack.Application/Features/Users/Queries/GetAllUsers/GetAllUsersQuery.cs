using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;

namespace BayTack.Application.Features.Users.Queries.GetAllUsers
{
	public sealed record GetAllUsersQuery(
	string? Search,
	string? Role,
	int Page,
	int Limit) : IQuery<PaginatedList<UserResponse>>;
}
