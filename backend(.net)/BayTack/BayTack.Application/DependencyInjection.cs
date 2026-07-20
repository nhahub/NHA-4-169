using BayTack.Application.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace BayTack.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			var assembly = typeof(DependencyInjection).Assembly;

			services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssembly(assembly);

				// BUG FIX: this was never registered, so no command's changes were ever
				// persisted anywhere in the app - every handler ran, returned Success, and the
				// tracked Add/Update/Remove was silently discarded because nothing ever called
				// SaveChangesAsync. Every "NOTE: no SaveChangesAsync call here - UnitOfWorkBehavior
				// does it automatically" comment across the command handlers assumed this line existed.
				cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));

				// Still off: FluentValidation validators exist per-feature but aren't wired into
				// the pipeline (AddValidatorsFromAssembly + this behavior both need to be on
				// together). Leaving that decision to be made deliberately rather than as a
				// side effect of this fix - flip both together when you're ready to test it.
				//cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
			});

			// services.AddValidatorsFromAssembly(assembly);

			return services;
		}
	}
}
