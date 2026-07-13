using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Common.Security;
using BayTack.Infrastructure.Common;
using BayTack.Infrastructure.Identity;
using BayTack.Infrastructure.Persistence;
using BayTack.Infrastructure.Repositorty;
using BayTack.Infrastructure.Repositorty.BayTack.Infrastructure.Repositorty;
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
		public static void AddPermissionPolicies(this IServiceCollection services)
		{
			services.AddAuthorization(options =>
			{
				foreach (var permission in Permissions.All.Select(p => p.Id))
				{
					options.AddPolicy(permission!, policy => policy.RequireClaim("Permission", permission!));
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

				// Add event handler for token validation (optional)
				
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
					}
				};

				options.Events = new JwtBearerEvents
				{
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
