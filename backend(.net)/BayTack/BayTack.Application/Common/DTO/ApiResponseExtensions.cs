using BayTack.Application.Common.Models;

namespace BayTack.Application.Common.DTO
{
    /// <summary>
    /// Maps the CQRS-layer Result / Result&lt;T&gt; into the ApiResponse&lt;T&gt; shape controllers
    /// return to clients. Kept here (not in the API project) so every controller behaves the
    /// same way instead of each one hand-rolling status codes.
    ///
    /// NOTE: the current Result/Result&lt;T&gt; only carries a plain string Error - there's no
    /// ErrorType (NotFound/Conflict/Validation/...) like the richer pattern discussed for this
    /// project. Until that's added, every failure maps to 400. Flagging this rather than
    /// guessing a status-code-from-message heuristic.
    /// </summary>
    public static class ApiResponseExtensions
    {
        public static ApiResponse<T> ToApiResponse<T>(this Result<T> result, string? traceId = null) => new()
        {
            IsSuccess = result.IsSuccess,
            Data = result.IsSuccess ? result.Value : default,
            Errors = result.IsSuccess ? null : result.Error,
            StatusCode = result.IsSuccess ? 200 : 400,
            TraceId = traceId
        };

        public static ApiResponse<object> ToApiResponse(this Result result, string? traceId = null) => new()
        {
            IsSuccess = result.IsSuccess,
            Data = null,
            Errors = result.IsSuccess ? null : result.Error,
            StatusCode = result.IsSuccess ? 200 : 400,
            TraceId = traceId
        };
    }
}