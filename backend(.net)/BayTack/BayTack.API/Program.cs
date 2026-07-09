using BayTack.Application;
using BayTack.Infrastructure;
using BayTack.Infrastructure.Identity;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddApplication();






var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//	app.MapOpenApi();
//}
using (var scope = app.Services.CreateScope())
	await Seeder.SeedAsync(scope.ServiceProvider);


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
