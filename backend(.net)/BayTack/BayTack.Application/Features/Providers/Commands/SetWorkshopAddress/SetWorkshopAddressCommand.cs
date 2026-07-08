using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Providers.Commands.SetWorkshopAddress
{
	public sealed record SetWorkshopAddressCommand(
		string ProviderProfileId,
		string Details,
		int CityId,
		int? AreaId,
		string UpdatedBy) : ICommand<SetWorkshopAddressResponse>;

	public sealed record SetWorkshopAddressResponse(
		string ProviderProfileId,
		string Details,
		int CityId,
		int? AreaId);
}
