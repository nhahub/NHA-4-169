using System.Threading.RateLimiting;

namespace BayTack.API.Extensions
{
	public static class RateLimiterExtensions
	{
		public static IServiceCollection AddCustomRateLimiter(this IServiceCollection services)
		{
			services.AddRateLimiter(options =>
			{
				// Global limiter - 100 requests per minute per IP
				options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
					RateLimitPartition.GetFixedWindowLimiter(
						partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
						factory: _ => new FixedWindowRateLimiterOptions
						{
							PermitLimit = 100,
							Window = TimeSpan.FromMinutes(1),
							QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
							QueueLimit = 5 // Allow a small queue for burst traffic
						}
					)
				);

				// Auth endpoints - stricter limit (10 per minute)
				options.AddPolicy("Auth", context =>
					RateLimitPartition.GetFixedWindowLimiter(
						partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
						factory: _ => new FixedWindowRateLimiterOptions
						{
							PermitLimit = 10,
							Window = TimeSpan.FromMinutes(1),
							QueueLimit = 0
						}
					)
				);

				// General API limiter
				options.AddPolicy("General", context =>
					RateLimitPartition.GetFixedWindowLimiter(
						partitionKey: context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
						factory: _ => new FixedWindowRateLimiterOptions
						{
							PermitLimit = 60,
							Window = TimeSpan.FromMinutes(1),
							QueueLimit = 3
						}
					)
				);

				options.OnRejected = async (context, token) =>
				{
					context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
					context.HttpContext.Response.ContentType = "application/json";

					var response = new
					{
						StatusCode = 429,
						Title = "Too Many Requests",
						Detail = "You have exceeded the rate limit. Please try again later.",
						RetryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
							? retryAfter.TotalSeconds
							: 60
					};

					await context.HttpContext.Response.WriteAsJsonAsync(response, token);
				};
			});

			return services;
		}
	}
}
