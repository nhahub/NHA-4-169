using BayTack.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Abstractions.Interfaces
{
	public interface IIdentityService
	{
		/// <summary>Creates the user with a generated temporary password and assigns the given role.
		/// Returns the new user's Id on success.</summary>
		Task<Result<string>> CreateUserAsync(string fullName, string email, string? phoneNumber, string role, CancellationToken ct = default);

		Task<Result<string>> UpdateUserAsync(string userId, string fullName, string email, string? phoneNumber, string? role, string updatedBy, CancellationToken ct = default);

		Task<Result<bool>> DeactivateUserAsync(string userId, CancellationToken ct = default);

		Task<Result<bool>> SoftDeleteUserAsync(string userId, string? deletedBy, string? reason, CancellationToken ct = default);
	}

}
