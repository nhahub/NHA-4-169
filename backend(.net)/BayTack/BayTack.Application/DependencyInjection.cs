using BayTack.Application.Abstractions.Interfaces;
using BayTack.Application.Common.Behaviors;
using BayTack.Application.EventMapping;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
namespace BayTack.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			var assembly = typeof(DependencyInjection).Assembly;

			//Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
			//Console.WriteLine(assembly);
			//foreach (var type in assembly.GetTypes())
			//{
			//	Console.WriteLine(type.Name);
			//}
			//Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");


			services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssembly(assembly);

				cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>));
				cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
				cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
				cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
			});
			
			 services.AddValidatorsFromAssembly(assembly);




			services.AddScoped<IIntegrationEventMapper, OrderIntegrationEventMapper>();

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
