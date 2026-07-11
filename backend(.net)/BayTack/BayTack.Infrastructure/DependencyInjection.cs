using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Abstractions.IRepository;
using BayTack.Infrastructure.Common;
using BayTack.Infrastructure.Identity;
using BayTack.Infrastructure.Persistence;
using BayTack.Infrastructure.Repositorty;
using BayTack.Infrastructure.Repositorty.BayTack.Infrastructure.Repositorty;
using BayTack.Infrastructure.Services.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BayTack.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{

			services.AddAuthenticationServices(configuration);

			services.AddDbContext<AppDbContext>((sp, options) =>
			{
				options.UseSqlServer(
					configuration.GetConnectionString("DefaultConnection"),
					sql => sql.EnableRetryOnFailure());
			});

			 
			


			services.AddScoped<ICurrentUserService, CurrentUserService>();




			services.AddScoped<IProviderVerificationReadRepository, ProviderVerificationReadRepository>();
			services.AddScoped(typeof(IRepository<,>), typeof(RepositoryBase<,>));
			services.AddScoped<IUserRepository, UserRepository>(); 
			services.AddScoped<IOrdersReadRepository, OrdersReadRepository>();
			services.AddScoped<IConversationRepository, ConversationRepository>();
			services.AddScoped<IProviderRepository, ProviderRepository>();
			services.AddScoped<IRoleRepository, RoleRepository>();


			//// Identity DB (separate)
			//services.AddJobSiteIdentity(configuration);



			// Unit of Work
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			return services;
		}
	}
}
