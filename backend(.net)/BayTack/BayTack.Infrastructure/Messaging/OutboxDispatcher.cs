using BayTack.Application.Abstractions.Interfaces;
using BayTack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.Json;

namespace BayTack.Infrastructure.Messaging
{
	public sealed class OutboxDispatcher : BackgroundService
	{
		private const int MaxRetryCount = 5;
		private static readonly TimeSpan PollInterval = TimeSpan.FromSeconds(5);

		private readonly IServiceScopeFactory _scopeFactory;
		private readonly ILogger<OutboxDispatcher> _logger;

		private static readonly Lazy<Dictionary<string, Type>> TypeCache = new(BuildTypeCache);

		public OutboxDispatcher(IServiceScopeFactory scopeFactory, ILogger<OutboxDispatcher> logger)
		{
			_scopeFactory = scopeFactory;
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					await DispatchPendingAsync(stoppingToken);
				}
				catch (Exception ex)
				{
					// A failure here means the poll itself blew up (e.g. DB unreachable) -
					// log and try again next tick rather than crashing the whole host.
					_logger.LogError(ex, "Outbox dispatch cycle failed");
				}

				await Task.Delay(PollInterval, stoppingToken);
			}
		}

		private async Task DispatchPendingAsync(CancellationToken ct)
		{
			using var scope = _scopeFactory.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
			var publisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();

			var pending = await db.OutboxMessages
				.Where(m => m.ProcessedOnUtc == null && m.RetryCount < MaxRetryCount)
				.OrderBy(m => m.OccurredOnUtc)
				.Take(50) // small batches - keep each poll cycle cheap and frequent
				.ToListAsync(ct);

			if (pending.Count == 0) return;

			foreach (var message in pending)
			{
				if (!TypeCache.Value.TryGetValue(message.Type, out var eventType))
				{
					_logger.LogError("Outbox message {Id} has unknown type {Type} - skipping, won't retry", message.Id, message.Type);
					message.Error = $"Unknown event type: {message.Type}";
					message.RetryCount = MaxRetryCount; // give up - a missing type won't fix itself on retry
					continue;
				}

				try
				{
					var integrationEvent = (IIntegrationEvent?)JsonSerializer.Deserialize(message.Content, eventType);
					if (integrationEvent is null) throw new InvalidOperationException("Deserialized to null");

					await publisher.PublishAsync(integrationEvent, ct);

					message.ProcessedOnUtc = DateTime.UtcNow;
					message.Error = null;
				}
				catch (Exception ex)
				{
					message.RetryCount++;
					message.Error = ex.Message;
					_logger.LogWarning(ex, "Failed to publish outbox message {Id} (attempt {Attempt}/{Max})",
						message.Id, message.RetryCount, MaxRetryCount);
				}
			}

			await db.SaveChangesAsync(ct);
		}

		/// <summary>Scans every loaded assembly once for IIntegrationEvent implementations,
		/// keyed by AssemblyQualifiedName (what AppDbContext.SaveChangesAsync stores on the
		/// Outbox row). Add a new integration event type and it just shows up here - nothing
		/// to register manually.</summary>
		private static Dictionary<string, Type> BuildTypeCache() =>
			AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a =>
				{
					try { return a.GetTypes(); }
					catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t != null)!; }
				})
				.Where(t => typeof(IIntegrationEvent).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
				.ToDictionary(t => t.AssemblyQualifiedName!, t => t);
	}
}
