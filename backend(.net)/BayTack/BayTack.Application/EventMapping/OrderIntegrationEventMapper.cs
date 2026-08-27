using BayTack.Application.Abstractions.Interfaces;
using BayTack.Contracts.IntegrationEvents.Orders;
using BayTack.Domain.Common.Events;
using BayTack.Domain.Entities.OrderAggregate.Event;

namespace BayTack.Application.EventMapping
{
	public sealed class OrderIntegrationEventMapper : IIntegrationEventMapper
	{
		public bool TryMap(IDomainEvent domainEvent, out IIntegrationEvent? integrationEvent)
		{
			switch (domainEvent)
			{
				case OrderCreatedDomainEvent e:
					integrationEvent = new OrderCreatedIntegrationEvent(
						OrderId: e.OrderId,
						CustomerJobId: e.CustomerJobId,
						ProviderId: e.ProviderId,
						FinalPriceAmount: e.FinalPrice.Amount,
						FinalPriceCurrency: e.FinalPrice.Currency,
						StartDate: e.StartDate,
						Status: nameof(Domain.Enums.OrderStatus.Pending));
					return true;

				case OrderStatusChangedDomainEvent e:
					integrationEvent = new OrderStatusChangedIntegrationEvent(
						OrderId: e.OrderId,
						NewStatus: e.NewStatus.ToString(),
						ChangedBy: e.ChangedBy,
						ChangedAt: e.OccurredOn);
					return true;

				default:
					integrationEvent = null;
					return false;
			}
		}
	}
}
