using Asp.Versioning.ApiExplorer;
using BayTack.API.Swagger;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BayTack.API.Swagger
{
	public class ConfigureSwaggerOptions
		: IConfigureOptions<SwaggerGenOptions>
	{
		private readonly IApiVersionDescriptionProvider _provider;

		public ConfigureSwaggerOptions(
			IApiVersionDescriptionProvider provider)
		{
			_provider = provider;
		}

		public void Configure(SwaggerGenOptions options)
		{
			foreach (var description
					 in _provider.ApiVersionDescriptions)
			{
				options.SwaggerDoc(
					description.GroupName,
					new OpenApiInfo
					{
						Title = $"BayTack API {description.ApiVersion}",


						Version = description.ApiVersion.ToString(),

						Description = description.IsDeprecated
							? "This API version is deprecated."
							: "BayTack Home Maintenance Marketplace Platform API",

						Contact = new OpenApiContact
						{
							Name = "BayTack Team",
							Email = "mahmoud.salah8411@gmail.com"
						},
						
					});

			}
			// ✅ أضف JWT Auth في Swagger
			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Name = "Authorization",
				Type = SecuritySchemeType.Http,
				Scheme = "Bearer",
				BearerFormat = "JWT",
				In = ParameterLocation.Header,
				Description = "Enter 'Bearer' [space] and then your valid token."
			});

			options.AddSecurityRequirement(document =>
				new OpenApiSecurityRequirement
				{
					[new OpenApiSecuritySchemeReference("Bearer", document)] = []
				});
		}
	}
}





