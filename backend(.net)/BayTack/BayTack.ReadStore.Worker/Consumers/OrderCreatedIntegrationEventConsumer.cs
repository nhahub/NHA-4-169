using BayTack.Contracts.IntegrationEvents.Orders;
using BayTack.ReadStore.Models;
using BayTack.ReadStore.Persistence;
using MassTransit;

namespace BayTack.ReadStore.Worker.Consumers
{

	public sealed class OrderCreatedIntegrationEventConsumer : IConsumer<OrderCreatedIntegrationEvent>
	{
		private readonly ReadDbContext _db;
		private readonly ILogger<OrderCreatedIntegrationEventConsumer> _logger;

		public OrderCreatedIntegrationEventConsumer(ReadDbContext db, ILogger<OrderCreatedIntegrationEventConsumer> logger)
		{
			_db = db;
			_logger = logger;
		}

		public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
		{
			var message = context.Message;
			var ct = context.CancellationToken;

			// Idempotency check first - if MassTransit redelivers this message (broker restart,
			// a previous attempt crashed after the DB commit but before acking), this makes the
			// second delivery a no-op instead of a duplicate row.
			if (await _db.ProcessedEvents.FindAsync(new object[] { message.EventId }, ct) is not null)
			{
				_logger.LogInformation("OrderCreated {EventId} already processed - skipping", message.EventId);
				return;
			}

			_db.Orders.Add(new OrderReadModel
			{
				OrderId = message.OrderId,
				CustomerId = message.CustomerId,
				CustomerJobId = message.CustomerJobId,
				ServiceId = message.ServiceId,
				Title = message.Title,
				Description = message.Description,
				ProviderId = message.ProviderId,
				ProviderName = message.ProviderName,
				FinalPriceAmount = message.FinalPriceAmount,
				FinalPriceCurrency = message.FinalPriceCurrency,
				StartDate = message.StartDate,
				Status = message.Status,
				CreatedAtUtc = message.OccurredOn
			});

			// Seeds the history list so GetByIdForCustomerAsync always has at least the initial
			// "Pending" entry, matching what the Write side's OrderStatusHistory does on Order.Create.
			_db.OrderHistory.Add(new OrderHistoryReadModel
			{
				OrderId = message.OrderId,
				Status = message.Status,
				ChangedBy = message.CustomerId,
				ChangedAtUtc = message.OccurredOn
			});

			_db.ProcessedEvents.Add(new ProcessedEvent
			{
				EventId = message.EventId,
				EventType = nameof(OrderCreatedIntegrationEvent),
				ProcessedAtUtc = DateTime.UtcNow
			});

			// All three inserts commit together - a message is never "half applied".
			await _db.SaveChangesAsync(ct);
		}
	}

}
