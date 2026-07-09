using Microsoft.Extensions.DependencyInjection;

namespace BayTack.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services )
		{

			services.AddMediatR(cfg =>
				cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

			//services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

			var assembly = typeof(DependencyInjection).Assembly;

			services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssembly(assembly);

				// إضافة بوابة التفتيش لتعمل مع كل طلب
				//cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
			});

			// تسجيل كل الـ Validators الموجودة في المشروع تلقائياً
			//services.AddValidatorsFromAssembly(assembly);


			return services;
		}
	}
}
