using BayTack.Application.Common.Security;
using BayTack.Domain.Entities.Location;
using BayTack.Domain.Entities.PaymentAggregate;
using BayTack.Domain.Entities.ServiceAggregate;
using BayTack.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace BayTack.Infrastructure.Identity
{
	public static class Seeder
	{
		// Change this password immediately after first login in any real environment -
		// it exists only so there's a way to log in on a freshly-seeded database.
		private const string DefaultAdminEmail = "admin@homeservices.local";
		private const string DefaultAdminPassword = "ChangeMe123!";

		public static async Task SeedAsync(IServiceProvider serviceProvider)
		{
			var context = serviceProvider.GetRequiredService<AppDbContext>();
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<string>>>();
			var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
			var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("ApplicationDbContextSeeder");

			await context.Database.MigrateAsync();

			await SeedRolesAsync(roleManager, logger);
			await SeedAdminUserAsync(userManager, logger);
			await SeedCitiesAsync(context, logger);
			await SeedPaymentMethodsAsync(context, logger);
			await SeedServiceCategoriesAsync(context, logger);

			await context.SaveChangesAsync();
		}

		/// <summary>
		/// Three starter roles with a reasonable default permission set. Re-runnable:
		/// creates the role if missing, and only adds permission claims that aren't
		/// already there yet - never removes claims an admin may have customized since.
		/// </summary>
		private static async Task SeedRolesAsync(RoleManager<IdentityRole<string>> roleManager, ILogger logger)
		{
			var roleDefinitions = new Dictionary<string, string[]>
			{
				["Admin"] = Permissions.All.Select(p => p.Id).ToArray(),
				["Provider"] = new[] { Permissions.JobsView, Permissions.ProvidersView },
				["Customer"] = new[] { Permissions.JobsView },
			};

			foreach (var (roleName, permissionIds) in roleDefinitions)
			{
				var role = await roleManager.FindByNameAsync(roleName);
				if (role is null)
				{
				    role = new IdentityRole<string>
					{
						Id = Guid.NewGuid().ToString(),  
						Name = roleName,
						NormalizedName = roleName.ToUpper()
					};

					var createResult = await roleManager.CreateAsync(role);
					if (!createResult.Succeeded)
					{
						logger.LogWarning("Failed to seed role {RoleName}: {Errors}",
							roleName, string.Join("; ", createResult.Errors.Select(e => e.Description)));
						continue;
					}
				}

				var existingPermissionIds = (await roleManager.GetClaimsAsync(role))
					.Where(c => c.Type == Permissions.ClaimType)
					.Select(c => c.Value)
					.ToHashSet();

				foreach (var permissionId in permissionIds.Where(p => !existingPermissionIds.Contains(p)))
					await roleManager.AddClaimAsync(role, new Claim(Permissions.ClaimType, permissionId));
			}
		}

		private static async Task SeedAdminUserAsync(UserManager<AppUser> userManager, ILogger logger)
		{
			var existing = await userManager.FindByEmailAsync(DefaultAdminEmail);
			if (existing is not null) return;

			var admin = AppUser.Create(userName: DefaultAdminEmail, email: DefaultAdminEmail, fullName: "System Administrator");
			var createResult = await userManager.CreateAsync(admin, DefaultAdminPassword);

			if (!createResult.Succeeded)
			{
				logger.LogWarning("Failed to seed default admin user: {Errors}",
					string.Join("; ", createResult.Errors.Select(e => e.Description)));
				return;
			}

			await userManager.AddToRoleAsync(admin, "Admin");
			logger.LogInformation("Seeded default admin user {Email} - change the password on first login.", DefaultAdminEmail);
		}

		private static async Task SeedCitiesAsync(AppDbContext context, ILogger logger)
		{
			if (await context.Cities.AnyAsync()) return;

			var cairo = City.Create("Cairo");
			cairo.AddArea("Nasr City");
			cairo.AddArea("Maadi");
			cairo.AddArea("Heliopolis");
			cairo.AddArea("Downtown");

			var giza = City.Create("Giza");
			giza.AddArea("Dokki");
			giza.AddArea("Mohandessin");
			giza.AddArea("6th of October");

			var alexandria = City.Create("Alexandria");
			alexandria.AddArea("Smouha");
			alexandria.AddArea("Miami");
			alexandria.AddArea("Sidi Gaber");

			// AddArea(name) sets Area.CityId = City.Id, which is still 0/default here
			// (not yet inserted) - same pattern already used by Order.Create() adding
			// its first OrderStatusHistory. EF Core's relationship fixup via the
			// Areas navigation collection resolves the real generated CityId at
			// SaveChanges time, regardless of the placeholder value.
			context.Cities.AddRange(cairo, giza, alexandria);
			logger.LogInformation("Seeded 3 cities with their areas.");
		}

		private static async Task SeedPaymentMethodsAsync(AppDbContext context, ILogger logger)
		{
			if (await context.PaymentMethods.AnyAsync()) return;

			context.PaymentMethods.AddRange(
				PaymentMethod.Create("Cash", "Pay in cash upon service completion"),
				PaymentMethod.Create("Credit Card", "Pay via Visa/Mastercard"),
				PaymentMethod.Create("Mobile Wallet", "Pay via mobile wallet (Vodafone Cash, Fawry, etc.)"));

			logger.LogInformation("Seeded 3 payment methods.");
		}

		private static async Task SeedServiceCategoriesAsync(AppDbContext context, ILogger logger)
		{
			if (await context.ServiceCategories.AnyAsync()) return;

			context.ServiceCategories.AddRange(
				ServiceCategory.Create("Plumbing", "Pipe repairs, installations, and leak fixes"),
				ServiceCategory.Create("Electrical", "Wiring, installations, and repairs"),
				ServiceCategory.Create("Cleaning", "Home and office cleaning services"),
				ServiceCategory.Create("Painting", "Interior and exterior painting"),
				ServiceCategory.Create("Carpentry", "Furniture repair and custom woodwork"));

			logger.LogInformation("Seeded 5 service categories.");
		}
	}
}
