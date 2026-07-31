using MediatR;
using Microsoft.Extensions.Logging;

namespace BayTack.Application.Common.Behaviors
{
	/// <summary>Logs every request in/out, including whether it resulted in an Result failure.</summary>
	public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

		public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
		{
			var requestName = typeof(TRequest).Name;
			_logger.LogInformation("Handling {Request}", requestName);

			var response = await next();

			_logger.LogInformation("Handled {Request}", requestName);
			return response;
		}
	}
}

