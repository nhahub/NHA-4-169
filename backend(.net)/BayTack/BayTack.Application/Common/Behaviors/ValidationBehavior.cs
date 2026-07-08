using System.Reflection;
using BayTack.Application.Common.Models;
using FluentValidation;
using MediatR;

namespace BayTack.Application.Common.Behaviors
{
    /// <summary>
    /// Runs all IValidator&lt;TRequest&gt; registered for the request before the handler executes.
    /// Works for both ICommand/ICommand&lt;T&gt;/IQuery&lt;T&gt; because TResponse is always either
    /// Result or Result&lt;T&gt;, and both expose a static Failure(string) factory - found via
    /// reflection once per failing request rather than requiring a second generic parameter.
    /// </summary>
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

        public async Task<TResponse> Handle(
            TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            if (!_validators.Any())
                return await next();

            var failures = new List<string>();
            foreach (var validator in _validators)
            {
                var result = await validator.ValidateAsync(request, ct);
                if (!result.IsValid)
                    failures.AddRange(result.Errors.Select(e => e.ErrorMessage));
            }

            if (failures.Count == 0)
                return await next();

            var errorMessage = string.Join(" | ", failures);

            var failureFactory = typeof(TResponse).GetMethod(
                nameof(Result.Failure), BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null)
                ?? throw new InvalidOperationException(
                    $"{typeof(TResponse).Name} has no static Failure(string) factory - it must be Result or Result<T>.");

            return (TResponse)failureFactory.Invoke(null, new object[] { errorMessage })!;
        }
    }
}