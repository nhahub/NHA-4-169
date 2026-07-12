using BayTack.Application;
using BayTack.Application.Abstractions.IRepository;
using BayTack.Infrastructure;
using BayTack.Infrastructure.Identity;
using BayTack.Infrastructure.Repositorty;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();


builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

builder.Services.AddHttpContextAccessor();


builder.Services.AddApplication();

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

using (var scope = app.Services.CreateScope())
{
	await Seeder.SeedAsync(scope.ServiceProvider);
}

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
