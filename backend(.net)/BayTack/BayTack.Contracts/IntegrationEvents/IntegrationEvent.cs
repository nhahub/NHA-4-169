using BayTack.Application.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Contracts.IntegrationEvents
{
	public abstract record IntegrationEvent : IIntegrationEvent
	{
		public Guid EventId { get; init; } = Guid.NewGuid();
		public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
	}
}
