using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using System.Security.Claims;

namespace BayTack.API.Extensions
{
	public static class SerilogExtensions
	{
		public static void UseBootstrapLogger()  
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateBootstrapLogger();
		}

		
		public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)  
		{
			builder.Host.UseSerilog((context, services, configuration) => configuration
				.ReadFrom.Configuration(context.Configuration)
				.ReadFrom.Services(services)
				.Enrich.FromLogContext()
				.Enrich.WithExceptionDetails()
				// for Entity Framework Core exceptions, we can use the EntityFrameworkCoreDestructurer to get more detailed information about the exception
				.Enrich.WithExceptionDetails(new DestructuringOptionsBuilder() 
				.WithDefaultDestructurers()
				.WithDestructurers(new[] { new DbUpdateExceptionDestructurer() })));

			return builder;
		}

		
		public static WebApplication UseRequestLogging(this WebApplication app)  
		{
			app.UseSerilogRequestLogging(options =>
			{
				options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

				options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
				{
					diagnosticContext.Set("TraceId", httpContext.TraceIdentifier);
					diagnosticContext.Set("Host", httpContext.Request.Host.Value);
					diagnosticContext.Set("Scheme", httpContext.Request.Scheme);
					diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress?.ToString());
					diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());

					var userId = httpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
					if (!string.IsNullOrEmpty(userId))
					{
						diagnosticContext.Set("UserId", userId);
					}
				};
			});

			return app;
		}
	}
}
