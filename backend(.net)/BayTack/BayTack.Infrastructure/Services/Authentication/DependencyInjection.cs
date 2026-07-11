using BayTack.Application.Abstractions.Interfaces;
using BayTack.Infrastructure.Identity;
using BayTack.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BayTack.Infrastructure.Services.Authentication
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<TokenSettings>(configuration.GetSection("TokenSettings"));
			services.AddScoped<ITokenService, TokenService>();


			services.AddScoped<IAuthService, AuthService>();

			services.AddScoped<IIdentityService, IdentityService>();



			services.AddIdentity<AppUser, IdentityRole<string>>(options =>
			{
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequiredLength = 6;
			})
			.AddEntityFrameworkStores<AppDbContext>()
			.AddDefaultTokenProviders();




			return services;
		}
	}
}
