using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Users.Queries.GetAllUsers
{
	public sealed record UserResponse(
	string Id,
	string FullName,
	string? Email,
	string? PhoneNumber,
	string Status,
	IReadOnlyList<string> Roles,
	DateTime CreatedAt);
}
