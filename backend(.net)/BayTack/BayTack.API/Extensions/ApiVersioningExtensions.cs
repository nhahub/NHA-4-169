using Asp.Versioning;

namespace BayTack.API.Extensions
{
	public static class ApiVersioningExtensions
	{
		public static IServiceCollection AddCustomApiVersioning(
			this IServiceCollection services)
		{
			services
				.AddApiVersioning(options =>
				{
					options.DefaultApiVersion = new ApiVersion(1, 0);

					options.AssumeDefaultVersionWhenUnspecified = true;

					// return api versions in response
					options.ReportApiVersions = true;

					options.ApiVersionReader = ApiVersionReader.Combine(
						new UrlSegmentApiVersionReader(),
						new HeaderApiVersionReader("X-API-Version"),
						new MediaTypeApiVersionReader("api-version")
					);
				})
				.AddMvc()
				.AddApiExplorer(options =>
				{
					// Swagger groups:
					// v1
					// v2
					// v3
					options.GroupNameFormat = "'v'VVV";

					// يحوّل:
					// api/v{version}/products
					// إلى:
					// api/v1/products
					options.SubstituteApiVersionInUrl = true;
				});

			return services;
		}
	}
}