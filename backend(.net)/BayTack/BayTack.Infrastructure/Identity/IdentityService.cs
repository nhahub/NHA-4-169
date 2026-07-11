using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BayTack.Infrastructure.Identity
{
	public class IdentityService : IIdentityService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole<string>> _roleManager;

		public IdentityService(UserManager<AppUser> userManager, RoleManager<IdentityRole<string>> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		
		public async Task<Result<string>> CreateUserAsync(string fullName, string email, string? phoneNumber, string role, CancellationToken ct = default)
		{
			if (!await _roleManager.RoleExistsAsync(role))
				return Result<string>.Failure("The specified role does not exist.");

			var existing = await _userManager.FindByEmailAsync(email);
			if (existing is not null)
				return Result<string>.Failure("A user with the specified email address already exists.");

			var user = AppUser.Create(userName: email, email: email, fullName: fullName);
			if (!string.IsNullOrWhiteSpace(phoneNumber))
				user.PhoneNumber = phoneNumber; // Identity's own property - can't be private-setter, framework owns it

			// No password in the request payload (admin-created account) - generate a
			// temporary one. TODO: wire this into an email "set your password" flow;
			// for now the temp password is only returned via logs/ops, never in the API response.
			var temporaryPassword = GenerateTemporaryPassword();

			var createResult = await _userManager.CreateAsync(user, temporaryPassword);
			if (!createResult.Succeeded)
				return Result<string>.Failure(string.Join("; ", createResult.Errors.Select(e => e.Description)));

			await _userManager.AddToRoleAsync(user, role);

			return Result<string>.Success(user.Id.ToString());
		}

		public async Task<Result<string>> UpdateUserAsync(string userId, string fullName, string email, string? phoneNumber, string? role, string updatedBy, CancellationToken ct = default)
		{
			var user = await _userManager.FindByIdAsync(userId.ToString());
			if (user is null)
				return Result<string>.Failure("User not found.");

			user.UpdateProfile(fullName, user.Address, updatedBy);

			if (!string.Equals(user.Email, email, StringComparison.OrdinalIgnoreCase))
			{
				var existing = await _userManager.FindByEmailAsync(email);
				if (existing is not null && existing.Id != userId)
					return Result<string>.Failure("A user with the specified email address already exists.");

				await _userManager.SetEmailAsync(user, email);
				await _userManager.SetUserNameAsync(user, email);
			}

			if (!string.IsNullOrWhiteSpace(phoneNumber))
				await _userManager.SetPhoneNumberAsync(user, phoneNumber);

			if (!string.IsNullOrWhiteSpace(role))
			{
				if (!await _roleManager.RoleExistsAsync(role))
					return Result<string>.Failure("The specified role does not exist.");

				var currentRoles = await _userManager.GetRolesAsync(user);
				await _userManager.RemoveFromRolesAsync(user, currentRoles);
				await _userManager.AddToRoleAsync(user, role);
			}

			var updateResult = await _userManager.UpdateAsync(user);
			if (!updateResult.Succeeded)
				return Result<string>.Failure(string.Join("; ", updateResult.Errors.Select(e => e.Description)));

			return Result<string>.Success(user.Id.ToString());
		}

		public async Task<Result<bool>> DeactivateUserAsync(string userId, CancellationToken ct = default)
		{
			var user = await _userManager.FindByIdAsync(userId.ToString());
			if (user is null)
				return Result<bool>.Failure("User not found.");

			user.Suspend(); // domain method on AppUser
			var result = await _userManager.UpdateAsync(user);

			return result.Succeeded
				? Result<bool>.Success(true)
				: Result<bool>.Failure("Failed to deactivate the user.");
		}

		public async Task<Result<bool>> SoftDeleteUserAsync(string userId, string? deletedBy, string? reason, CancellationToken ct = default)
		{
			var user = await _userManager.FindByIdAsync(userId.ToString());
			if (user is null)
				return Result<bool>.Failure("User not found.");

			user.SoftDelete(deletedBy, reason); // domain method - IsDeleted, DeletedAt, DeletedBy, DeleteReason
			var result = await _userManager.UpdateAsync(user);

			return result.Succeeded
				? Result<bool>.Success(true)
				: Result<bool>.Failure("Failed to delete the user.");
		}

		private static string GenerateTemporaryPassword()
		{
			// Satisfies a typical ASP.NET Identity password policy (upper/lower/digit/special, 12+ chars)
			// without any obviously-guessable pattern. Replace with your own policy/generator if needed.
			const string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
			const string lower = "abcdefghijkmnpqrstuvwxyz";
			const string digits = "23456789";
			const string special = "!@#$%^&*";
			var all = upper + lower + digits + special;

			Span<char> result = stackalloc char[16];
			result[0] = upper[RandomNumberGenerator.GetInt32(upper.Length)];
			result[1] = lower[RandomNumberGenerator.GetInt32(lower.Length)];
			result[2] = digits[RandomNumberGenerator.GetInt32(digits.Length)];
			result[3] = special[RandomNumberGenerator.GetInt32(special.Length)];
			for (var i = 4; i < result.Length; i++)
				result[i] = all[RandomNumberGenerator.GetInt32(all.Length)];

			return new string(result);
		}

	}
}
