using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.SetAvailability
{
	public sealed class SetAvailabilityCommandHandler
		: ICommandHandler<SetAvailabilityCommand, SetAvailabilityResponse>
	{
		private readonly IRepository<ProviderProfile, string> _providerProfileRepository;
		private readonly IUnitOfWork _unitOfWork;

		public SetAvailabilityCommandHandler(
			IRepository<ProviderProfile, string> providerProfileRepository,
			IUnitOfWork unitOfWork)
		{
			_providerProfileRepository = providerProfileRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<SetAvailabilityResponse>> Handle(
			SetAvailabilityCommand request, CancellationToken ct)
		{
			var profile = await _providerProfileRepository.GetByIdAsync(request.ProviderProfileId, ct);

			if (profile is null)
				return Result<SetAvailabilityResponse>.Failure("Provider profile not found.");

			try
			{
				profile.SetAvailability(request.DayOfWeek, request.StartTime, request.EndTime);
			}
			catch (ArgumentException ex)
			{
				return Result<SetAvailabilityResponse>.Failure(ex.Message);
			}

			_providerProfileRepository.Update(profile);
			await _unitOfWork.SaveChangesAsync(ct);

			var response = new SetAvailabilityResponse(
				profile.Id,
				request.DayOfWeek.ToString(),
				request.StartTime.ToString(),
				request.EndTime.ToString());

			return Result<SetAvailabilityResponse>.Success(response);
		}
	}
}
