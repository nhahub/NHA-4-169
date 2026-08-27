using BayTack.Application.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Messaging
{
	public sealed class MassTransitEventPublisher : IEventPublisher
	{
		private readonly IPublishEndpoint _publishEndpoint;

		public MassTransitEventPublisher(IPublishEndpoint publishEndpoint)
		{
			_publishEndpoint = publishEndpoint;
		}

		public Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
			// MassTransit publishes to an exchange named after the event's concrete type
			// (e.g. "order-created-integration-event") and fans it out to every bound queue.
			// object overload is used (not the generic one) because we only have the
			// interface type at this call site - MassTransit resolves the runtime type fine.
			=> _publishEndpoint.Publish(integrationEvent, integrationEvent.GetType(), cancellationToken);
	}
}
