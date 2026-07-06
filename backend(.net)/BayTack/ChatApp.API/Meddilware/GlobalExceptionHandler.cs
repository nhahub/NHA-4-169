namespace ChatApp.API.Meddilware
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
                    response.Message = "Failed";
                    response.Errors = ex.Message;
                    response.ErrorCode = ErrorCodes.Validation;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    break;

                case DatabaseException:
                    response.Message = "Database error occurred";
                    response.Errors = ex.Message;
                    response.ErrorCode = ErrorCodes.ServerError;
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;

                case DomainException or BusinessRuleViolationException:
                    response.Message = "Failed";
                    response.Errors = ex.Message;
                    response.ErrorCode = ErrorCodes.businessRule;
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    break;

                //case NotFoundException:
                //	response.Message = ex.Message;
                //	response.ErrorCode = ErrorCodes.NotFound;
                //	response.StatusCode = StatusCodes.Status404NotFound;
                //	break;

                //case UnauthorizedException:
                //	response.Message = ex.Message;
                //	response.ErrorCode = ErrorCodes.Unauthorized;
                //	response.StatusCode = StatusCodes.Status401Unauthorized;
                //	break;

                //case ConflictException:
                //	response.Message = ex.Message;
                //	response.ErrorCode = ErrorCodes.Conflict;
                //	response.StatusCode = StatusCodes.Status409Conflict;
                //	break;

                default:
                    response.Message = $"Internal Server Error : ({ex.Message}) ";
                    response.Errors = ex.Message;
                    response.ErrorCode = ErrorCodes.ServerError;
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
