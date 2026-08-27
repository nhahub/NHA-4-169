using BayTack.Domain.Common.Events;

namespace BayTack.Application.Abstractions.Interfaces
{
	public interface IIntegrationEventMapper
	{
		bool TryMap(IDomainEvent domainEvent, out IIntegrationEvent? integrationEvent);
	}
}
