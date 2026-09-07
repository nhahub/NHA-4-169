//using BayTack.ReadStore.Worker;

//var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();

//var host = builder.Build();
//host.Run();



//Log.Logger = new LoggerConfiguration()
//	.WriteTo.Console()
//	.CreateLogger();

using BayTack.ReadStore.Persistence;
using BayTack.ReadStore.Worker;
using BayTack.ReadStore.Worker.Consumers;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Serilog;
using System;

try
{
    var builder = WebApplication.CreateBuilder(args);
	//builder.AddSerilogLogging();

	builder.Services.AddDbContext<ReadDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ReadDbConnection")));

	builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(RabbitMqOptions.SectionName));

	builder.Services.AddMassTransit(x =>
	{
		// Register every consumer for this bounded context here. Adding a new integration
		// event to react to just means adding one more IConsumer<T> class + one line here -
		// MassTransit creates its queue and binds it automatically via ConfigureEndpoints.
		x.AddConsumer<OrderCreatedIntegrationEventConsumer>();
		//x.AddConsumer<OrderStatusChangedIntegrationEventConsumer>();
		//x.AddConsumer<NotificationCreatedIntegrationEventConsumer>();
		//x.AddConsumer<NotificationMarkedReadIntegrationEventConsumer>();
		//x.AddConsumer<ServiceListingCreatedIntegrationEventConsumer>();
		//x.AddConsumer<ServiceListingUpdatedIntegrationEventConsumer>();

		// Use KebabCase for the queue names, e.g. "baytack.readstore.order-created-integration-event-consumer"
		x.SetKebabCaseEndpointNameFormatter();

		x.UsingRabbitMq((context, cfg) =>
		{
			var options = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

			cfg.Host(options.Host, options.VirtualHost, h =>
			{
				h.Username(options.Username);
				h.Password(options.Password);
			});

			
			cfg.UseMessageRetry(retry => retry.Exponential(
				retryLimit: 5,
				minInterval: TimeSpan.FromSeconds(1),
				maxInterval: TimeSpan.FromSeconds(30),
				intervalDelta: TimeSpan.FromSeconds(5)));

			cfg.ConfigureEndpoints(context);
		});
	});

	var rabbitMqConnectionString =
		$"amqp://{builder.Configuration["RabbitMq:Username"]}:{builder.Configuration["RabbitMq:Password"]}" +
		$"@{builder.Configuration["RabbitMq:Host"]}{builder.Configuration["RabbitMq:VirtualHost"]}";


	builder.Services.AddHealthChecks()
		.AddSqlServer(builder.Configuration.GetConnectionString("ReadDbConnection")!, name: "read-db")
		.AddRabbitMQ(
			factory: sp =>
			{
				var factory = new RabbitMQ.Client.ConnectionFactory      // for use one connection for all health checks, instead of creating a new one each time
				{
					Uri = new Uri(rabbitMqConnectionString)
				};
				return factory.CreateConnectionAsync().GetAwaiter().GetResult();
			},
			name: "rabbitmq"
		);


	var app = builder.Build();
	  
	// Dev convenience only - swap for EF migrations before this ever touches a shared
	// environment, same as any other EnsureCreated usage.
	using (var scope = app.Services.CreateScope())
	{
		var db = scope.ServiceProvider.GetRequiredService<ReadDbContext>();
		await db.Database.EnsureCreatedAsync();
	}

	app.MapHealthChecks("/health", new HealthCheckOptions
	{
		ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
	});

	Log.Information("Starting BayTack ReadStore Worker");
	await app.RunAsync();
}
catch (Exception ex)
{
	Log.Fatal(ex, "BayTack ReadStore Worker terminated unexpectedly");
}
finally
{
	Log.CloseAndFlush();
}
