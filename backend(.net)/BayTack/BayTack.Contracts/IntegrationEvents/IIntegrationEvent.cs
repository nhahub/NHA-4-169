
namespace BayTack.Application.Abstractions.Interfaces
{
	public interface IIntegrationEvent
	{
		Guid EventId { get; }
		DateTime OccurredOn { get; }
	}
}
