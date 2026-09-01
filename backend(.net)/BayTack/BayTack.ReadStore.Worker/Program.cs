//using BayTack.ReadStore.Worker;

//var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();

//var host = builder.Build();
//host.Run();



//Log.Logger = new LoggerConfiguration()
//	.WriteTo.Console()
//	.CreateLogger();

using BayTack.ReadStore.Persistence;

try
{
	var builder = WebApplication.CreateBuilder(args);
	builder.Services.AddSerilog();

	builder.Services.AddDbContext<ReadDbContext>(options =>
		options.UseSqlServer(builder.Configuration.GetConnectionString("ReadDbConnection")));

	builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection(RabbitMqOptions.SectionName));

	builder.Services.AddMassTransit(x =>
	{
		// Register every consumer for this bounded context here. Adding a new integration
		// event to react to just means adding one more IConsumer<T> class + one line here -
		// MassTransit creates its queue and binds it automatically via ConfigureEndpoints.
		x.AddConsumer<OrderCreatedIntegrationEventConsumer>();
		x.AddConsumer<OrderStatusChangedIntegrationEventConsumer>();
		x.AddConsumer<NotificationCreatedIntegrationEventConsumer>();
		x.AddConsumer<NotificationMarkedReadIntegrationEventConsumer>();
		x.AddConsumer<ServiceListingCreatedIntegrationEventConsumer>();
		x.AddConsumer<ServiceListingUpdatedIntegrationEventConsumer>();

		x.SetKebabCaseEndpointNameFormatter();

		x.UsingRabbitMq((context, cfg) =>
		{
			var options = context.GetRequiredService<Microsoft.Extensions.Options.IOptions<RabbitMqOptions>>().Value;

			cfg.Host(options.Host, options.VirtualHost, h =>
			{
				h.Username(options.Username);
				h.Password(options.Password);
			});

			// Same retry shape as the publisher side (Infrastructure.DependencyInjection) -
			// keep these in sync if you tune one.
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
		.AddRabbitMQ(rabbitMqConnectionString, name: "rabbitmq");

	var host = builder.Build();

	// Dev convenience only - swap for EF migrations before this ever touches a shared
	// environment, same as any other EnsureCreated usage.
	using (var scope = host.Services.CreateScope())
	{
		var db = scope.ServiceProvider.GetRequiredService<ReadDbContext>();
		await db.Database.EnsureCreatedAsync();
	}

	host.MapHealthChecks("/health", new HealthCheckOptions
	{
		ResponseWriter = HealthChecks.UI.Client.UIResponseWriter.WriteHealthCheckUIResponse
	});

	Log.Information("Starting BayTack ReadStore Worker");
	await host.RunAsync();
}
catch (Exception ex)
{
	Log.Fatal(ex, "BayTack ReadStore Worker terminated unexpectedly");
}
finally
{
	Log.CloseAndFlush();
}
