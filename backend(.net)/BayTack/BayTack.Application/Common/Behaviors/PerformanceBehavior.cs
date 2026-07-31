using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BayTack.Application.Common.Behaviors
{

	/// <summary>Warns when a request takes longer than 500ms — cheap way to catch
	/// missing indexes or N+1 queries before they hit production.</summary>
	public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		private const int WarningThresholdMs = 500;
		private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

		public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger) => _logger = logger;

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
		{
			var sw = Stopwatch.StartNew();
			var response = await next();
			sw.Stop();

			if (sw.ElapsedMilliseconds > WarningThresholdMs)
			{
				_logger.LogWarning("Slow request: {Request} took {ElapsedMs}ms",
					typeof(TRequest).Name, sw.ElapsedMilliseconds);
			}

			return response;
		}
	}

}
