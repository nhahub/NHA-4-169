using Asp.Versioning.ApiExplorer;
using BayTack.API.Extensions;
using BayTack.API.Middlewares;
using BayTack.API.Swagger;
using BayTack.Application;
using BayTack.Infrastructure;
using BayTack.Infrastructure.Identity;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Serialization;

try
{

	Log.Information("Starting BayTack API");

	SerilogExtensions.UseBootstrapLogger();

	var builder = WebApplication.CreateBuilder(args);

	builder.AddSerilogLogging();

	// Add services to the container.
	builder.Services.AddControllers()
		.AddJsonOptions(options =>
		{
			options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());// convert enum to string in json response
		});


	builder.Services.AddApplication();
	builder.Services.AddInfrastructure(builder.Configuration);
	builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
	builder.Services.AddProblemDetails();
	builder.Services.AddHttpContextAccessor();

	// cors
	var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
		?? new[] { "http://localhost:5500", "http://127.0.0.1:5500" };
	builder.Services.AddCors(options =>
	{
		options.AddPolicy("Frontend", policy =>
		{
			policy.WithOrigins(allowedOrigins)
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials();
		});
	});

	// Rate Limiting
	builder.Services.AddCustomRateLimiter();

	builder.Services.AddCustomApiVersioning();

	// for configration of swagger
	builder.Services.AddSwaggerGen();

	builder.Services.AddTransient< IConfigureOptions<SwaggerGenOptions>, 
	ConfigureSwaggerOptions>();







	var app = builder.Build();





	app.UseRequestLogging();// for logging request and response details
	
	// Rate Limiter (before auth)
	app.UseRateLimiter();

	app.UseExceptionHandler();

	using (var scope = app.Services.CreateScope())
	{
		await Seeder.SeedAsync(scope.ServiceProvider);
	}

	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		var provider = app.Services
			.GetRequiredService<IApiVersionDescriptionProvider>();

		foreach (var description in provider.ApiVersionDescriptions)
		{
			options.SwaggerEndpoint(
				$"/swagger/{description.GroupName}/swagger.json",
				$"BayTack API {description.GroupName.ToUpperInvariant()}");
		}
	});

	app.UseHttpsRedirection();

	app.UseCors("Frontend");

	app.UseAuthentication();
	app.UseAuthorization();

	app.MapControllers();

	app.Run();

}
catch (Exception ex) when (ex is not HostAbortedException)
{
	//Console.WriteLine(ex.ToString());

	//if (ex.InnerException != null)
	//{
	//	Console.WriteLine("Inner Exception:");
	//	Console.WriteLine(ex.InnerException.ToString());
	//}

	Log.Fatal(ex, "BayTack API terminated unexpectedly");
}
finally
{
	Log.CloseAndFlush();
}