using BayTack.Application.Abstractions.Interfaces;
using MassTransit;

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
			=> _publishEndpoint.Publish(integrationEvent, integrationEvent.GetType(), cancellationToken);
	}
}
