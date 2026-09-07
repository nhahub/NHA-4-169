namespace BayTack.Contracts.IntegrationEvents.Orders
{

	public sealed record OrderCreatedIntegrationEvent(
		string OrderId,
		string CustomerId,
		string CustomerJobId,
		string ServiceId,
		string Title,
		string Description,
		string ProviderId,
		string ProviderName,
		decimal FinalPriceAmount,
		string FinalPriceCurrency,
		DateTime StartDate,
		string Status
		) : IntegrationEvent;

}