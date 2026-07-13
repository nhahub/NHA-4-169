using BayTack.Application.Common.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace BayTack.API.Middlewares
{
	public class GlobalExceptionHandler : IExceptionHandler
	{
		private readonly ILogger<GlobalExceptionHandler> _logger;

		public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
		{
			_logger = logger;
		}

		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
		{
			_logger.LogError(ex, "Unexpected Error: {Message}", ex.Message);

			var traceId = httpContext.TraceIdentifier;

			var response = new ApiResponse<object>
			{
				IsSuccess = false,
				TraceId = traceId
			};

			switch (ex)
			{
				case ValidationException:
					response.Message = "Validation error occurred";
					response.Errors = ex.Message;
					response.ErrorCode = "400";
					response.StatusCode = StatusCodes.Status400BadRequest;
					break;


				default:
					response.Message = $"Internal Server Error : ({ex.Message}) ";
					response.Errors = ex.Message;
					response.ErrorCode = "500";
					response.StatusCode = StatusCodes.Status500InternalServerError;
					break;
			}

			httpContext.Response.StatusCode = response.StatusCode;
			httpContext.Response.ContentType = "application/json";

			await httpContext.Response.WriteAsJsonAsync(response);

			return true;
		}
	}
}
