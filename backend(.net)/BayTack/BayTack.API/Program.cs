using BayTack.API.Extensions;
using BayTack.API.Middlewares;
using BayTack.Application;
using BayTack.Infrastructure;
using BayTack.Infrastructure.Identity;
using Microsoft.OpenApi;
using Serilog;
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



	builder.Services.AddSwaggerGen(doc =>
	{
		//var xmlFile = Path.Combine(AppContext.BaseDirectory, "ApiDocumentation.xml");
		//doc.IncludeXmlComments(xmlFile);
		doc.SwaggerDoc("v1",
			new OpenApiInfo
			{
				Version = "v1",
				Title = "Jooobs API",
				Description = "API for managing job applications, user profiles, and related functionalities.",
				Contact = new OpenApiContact
				{
					Name = "mahmoud mohamed salah"
				}
			}
		);
	});







	var app = builder.Build();





	app.UseRequestLogging();


	app.UseExceptionHandler();

	using (var scope = app.Services.CreateScope())
	{
		await Seeder.SeedAsync(scope.ServiceProvider);
	}

	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();

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