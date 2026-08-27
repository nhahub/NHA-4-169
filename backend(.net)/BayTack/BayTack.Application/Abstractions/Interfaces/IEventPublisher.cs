using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Abstractions.Interfaces
{
	public interface IEventPublisher
	{
		Task PublishAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken = default);
	}
}
