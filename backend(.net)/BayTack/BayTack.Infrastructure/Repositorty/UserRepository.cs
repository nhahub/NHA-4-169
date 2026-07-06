using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Repositorty
{
	
	public sealed class UserRepository : IUserRepository
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<(IReadOnlyList<UserResponse> Items, int TotalCount)> SearchAsync(
			string? search, string? role, int page, int limit, CancellationToken cancellationToken)
		{
			IQueryable<ApplicationUser> query = _context.Users.AsNoTracking();

			if (!string.IsNullOrWhiteSpace(search))
			{
				query = query.Where(u =>
					u.FullName.Contains(search) ||
					(u.Email != null && u.Email.Contains(search)));
			}

			if (!string.IsNullOrWhiteSpace(role))
			{
				var roleId = await _context.Roles
					.Where(r => r.Name == role)
					.Select(r => r.Id)
					.FirstOrDefaultAsync(cancellationToken);

				if (roleId == default)
					return (Array.Empty<UserResponse>(), 0); // role مش موجود -> صفحة فاضية

				query = query.Where(u => _context.UserRoles
					.Any(ur => ur.UserId == u.Id && ur.RoleId == roleId));
			}

			var totalCount = await query.CountAsync(cancellationToken);

			var pagedUsers = await query
				.OrderByDescending(u => u.CreatedAt)
				.Skip((page - 1) * limit)
				.Take(limit)
				.ToListAsync(cancellationToken);

			var items = new List<UserResponse>(pagedUsers.Count);
			foreach (var user in pagedUsers)
			{
				var roles = await _userManager.GetRolesAsync(user);
				items.Add(new UserResponse(
					user.Id, user.FullName, user.Email,
					user.Status.ToString(), roles.ToList(), user.CreatedAt));
			}

			return (items, totalCount);
		}
	}
}
