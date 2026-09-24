using BayTack.ReadStore.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BayTack.ReadStore
{
	public static class DependencyInjection
	{


		public static IServiceCollection AddReadStore(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<ReadDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("ReadDbConnection")));
			return services;
		}

	}
}
