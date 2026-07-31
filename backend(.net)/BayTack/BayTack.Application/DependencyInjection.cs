using BayTack.Application.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
namespace BayTack.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			var assembly = typeof(DependencyInjection).Assembly;

			Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
			Console.WriteLine(assembly);
			foreach (var type in assembly.GetTypes())
			{
				Console.WriteLine(type.FullName);
			}
			Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");


			services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssembly(assembly);

				cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
				cfg.AddOpenBehavior(typeof(ValidationBehavior<,>)); 
				cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>)); 



			});
			
			 services.AddValidatorsFromAssembly(assembly);


			return services;











			//services.AddValidatorsFromAssembly(assembly);

			//services.AddMediatR(cfg =>
			//{
			//	cfg.RegisterServicesFromAssembly(assembly);

			//	cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
			//	cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
			//	cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
			//	cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>));
			//});

		}
	}
}
