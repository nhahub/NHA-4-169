using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Abstractions.IRepository;
using BayTack.Infrastructure.Identity;
using BayTack.Infrastructure.Persistence;
using BayTack.Infrastructure.Repositorty;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BayTack.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
		{



			//services.AddDbContext<AppDbContext>((sp, options) =>
			//{

			//	options.UseSqlServer(
			//		configuration.GetConnectionString("DefaultConnection"),
			//		sql => sql.EnableRetryOnFailure());
			//});




			//// Identity DB (separate)
			//services.AddJobSiteIdentity(configuration);

			// Repositories
			//services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
			//services.AddScoped<IUserRepository, UserRepository>();
			//services.AddScoped<ICompanyRepository, CompanyRepository>();
			//services.AddScoped<IJobRepository, JobRepository>();
			//services.AddScoped<IApplicationRepository, ApplicationRepository>();
			//services.AddScoped<ICVRepository, CVRepository>();
			//services.AddScoped<ISkillRepository, SkillRepository>();
			//services.AddScoped<ICVJobRecommendationRepository, CVJobRecommendationRepository>();
			//services.AddScoped<IUserSkillRepository, UserSkillsRepository>();


			// Unit of Work
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			return services;
		}
	}
}
