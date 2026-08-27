using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Abstractions.Interfaces
{
	public interface IIntegrationEvent
	{
		Guid EventId { get; }
		DateTime OccurredOn { get; }
	}

}
