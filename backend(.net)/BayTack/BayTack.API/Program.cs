using BayTack.API.Middlewares;
using BayTack.Application;
using BayTack.Application.Abstractions.IRepository;
using BayTack.Infrastructure;
using BayTack.Infrastructure.Identity;
using BayTack.Infrastructure.Repositorty;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		// Lets clients send/receive enum fields (e.g. ProviderType) as strings like
		// "Individual"/"Company" instead of raw integers - without this, the default
		// System.Text.Json behavior rejects string enum values in request bodies with a 400.
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();


builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

builder.Services.AddHttpContextAccessor();


builder.Services.AddApplication();

// CORS: required because the frontend and backend live on different domains.
// AllowCredentials is needed for the httpOnly auth cookies to be sent/received
// cross-site, which means we cannot use AllowAnyOrigin() - origins must be
// listed explicitly. Add every frontend URL that will call this API
// (local dev + the deployed frontend domain) to appsettings under "Cors:AllowedOrigins".
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
		});

});

var app = builder.Build();

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
