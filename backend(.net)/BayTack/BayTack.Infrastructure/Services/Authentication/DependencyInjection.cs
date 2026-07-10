using BayTack.Application.Abstractions.Interfaces;
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
			return services;
		}
	}
}
