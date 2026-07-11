


using BayTack.Infrastructure;
using BayTack.Application;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();


builder.Services.AddInfrastructure(builder.Configuration);



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

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//	app.MapOpenApi();
//}
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
