namespace BayTack.Contracts.IntegrationEvents.Orders
{
	public sealed record OrderStatusChangedIntegrationEvent(
		string OrderId,
		string NewStatus,
		string ChangedBy,
		DateTime ChangedAt
	) : IntegrationEvent;
}