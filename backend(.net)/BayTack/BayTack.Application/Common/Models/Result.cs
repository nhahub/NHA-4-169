namespace BayTack.Application.Common.Models
{
    /// <summary>
    /// Common shape shared by <see cref="Result"/> and <see cref="Result{T}"/> so that
    /// cross-cutting code (pipeline behaviors, API mapping) can inspect success/error
    /// without knowing which one it's holding.
    /// </summary>
    public interface IResult
    {
        bool IsSuccess { get; }
        string? Error { get; }
    }

    /// <summary>Non-payload result for commands that don't return a value (e.g. MarkAllNotificationsRead).</summary>
    public class Result : IResult
    {
        public bool IsSuccess { get; }
        public string? Error { get; }

        private Result(bool isSuccess, string? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, null);
        public static Result Failure(string error) => new(false, error);
        public static Result NotFound(string error) => new(false, error);
    }

    public class Result<T> : IResult
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? Error { get; }

        private Result(bool isSuccess, T? value, string? error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value) => new(true, value, null);
        public static Result<T> Failure(string error) => new(false, default, error);
        public static Result<T> NotFound(string error) => new(false, default, error);

        public static implicit operator Result<T>(T value) => Success(value);
    }
}