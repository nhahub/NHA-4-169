using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Common.Security;
using BayTack.Infrastructure.Common;
using BayTack.Infrastructure.Identity;
using BayTack.Infrastructure.Persistence;
using BayTack.Infrastructure.Repositorty;
using BayTack.Infrastructure.Services.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BayTack.Infrastructure
{
	public static class DependencyInjection
	{
		/// <summary>Controllers use [Authorize(Policy = "Permissions.Categories.View")] - PascalCase,
		/// dot-separated, "Permissions." prefix - built from a Permissions.Id like "categories.view"
		/// (lowercase, dot/underscore-separated). This turns one into the other so the registered
		/// policy names actually match what's on the controllers.</summary>
		public static string ToPolicyName(string permissionId) =>
			"Permissions." + string.Join(".", permissionId.Split('.').Select(segment =>
				string.Concat(segment.Split('_').Select(word =>
					char.ToUpperInvariant(word[0]) + word[1..]))));

		public static void AddPermissionPolicies(this IServiceCollection services)
		{
			services.AddAuthorization(options =>
			{
				foreach (var permission in Permissions.All)
				{
					// BUG FIX: this used to register policies under the bare permission.Id
					// ("categories.view") and require a claim of type "Permission" (capital P) -
					// neither matched what the controllers or the seeded claims actually use
					// (see ToPolicyName above, and Permissions.ClaimType = "permission", lowercase).
					options.AddPolicy(ToPolicyName(permission.Id),
						policy => policy.RequireClaim(Permissions.ClaimType, permission.Id));
				}
			});
		}
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddPermissionPolicies();


			services.AddDbContext<AppDbContext>((sp, options) =>
			{
				options.UseSqlServer(
					configuration.GetConnectionString("DefaultConnection"),
					sql => sql.EnableRetryOnFailure());
			});



			services.AddIdentity<AppUser, IdentityRole<string>>(options =>
			{
				// password settings
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 8;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = true;
				// lockout settings
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.AllowedForNewUsers = true;
				// user settings
				options.User.RequireUniqueEmail = true;
			})
			.AddEntityFrameworkStores<AppDbContext>()
			.AddDefaultTokenProviders();


			services
			.AddAuthentication(options => {
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options => {
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,

					ValidIssuer = configuration["TokenSettings:Issuer"],
					ValidAudience = configuration["TokenSettings:Audience"],
					ClockSkew = TimeSpan.Zero,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenSettings:SecretKey"]!))
				};

				// BUG FIX: these two handlers used to be set via two separate
				// `options.Events = new JwtBearerEvents { ... }` assignments - the second
				// one silently replaced the first, so OnMessageReceived (which reads the
				// access token out of the httpOnly cookie) never ran. Every [Authorize]
				// endpoint would fail authentication because no Authorization header is
				// sent by the frontend and the cookie was never being read. Merged into
				// a single JwtBearerEvents instance so both handlers actually run.
				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						var accessToken = context.Request.Cookies["accessToken"];
						if (!string.IsNullOrEmpty(accessToken))
						{
							context.Token = accessToken;
						}
						return Task.CompletedTask;
					},
					OnChallenge = async context =>
					{
						context.HandleResponse();

						context.Response.StatusCode = StatusCodes.Status401Unauthorized;
						context.Response.ContentType = "application/json";

						await context.Response.WriteAsJsonAsync(new
						{
							message = "Unauthorized access",
							errorCode = "401"
						});
					}
				};

			});


			services.Configure<TokenSettings>(configuration.GetSection("TokenSettings"));

			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IIdentityService, IdentityService>();
			services.AddScoped<ICurrentUserService, CurrentUserService>();
			services.AddScoped<IProviderVerificationReadRepository, ProviderVerificationReadRepository>();
			services.AddScoped<IProviderDashboardReadRepository, ProviderDashboardReadRepository>();
			services.AddScoped(typeof(IRepository<,>), typeof(RepositoryBase<,>));
			services.AddScoped<IUserRepository, UserRepository>(); 
			services.AddScoped<IOrdersReadRepository, OrdersReadRepository>();
			services.AddScoped<IConversationRepository, ConversationRepository>();
			services.AddScoped<IProviderRepository, ProviderRepository>();
			services.AddScoped<IRoleRepository, RoleRepository>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			return services;
		}
	}
}
