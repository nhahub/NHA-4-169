namespace BayTack.Contracts.IntegrationEvents.Orders
{
	public sealed record OrderCreatedIntegrationEvent(
		string OrderId,
		string CustomerJobId,
		string ProviderId,
		decimal FinalPriceAmount,
		string FinalPriceCurrency,
		DateTime StartDate,
		string Status
	) : IntegrationEvent;
}
