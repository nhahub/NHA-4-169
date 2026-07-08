using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.VerifyProvider
{
	public sealed class VerifyProviderCommandHandler
		: ICommandHandler<VerifyProviderCommand, VerifyProviderResponse>
	{
		private readonly IRepository<ProviderProfile, string> _providerProfileRepository;
		private readonly IUnitOfWork _unitOfWork;

		public VerifyProviderCommandHandler(
			IRepository<ProviderProfile, string> providerProfileRepository,
			IUnitOfWork unitOfWork)
		{
			_providerProfileRepository = providerProfileRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<VerifyProviderResponse>> Handle(
			VerifyProviderCommand request, CancellationToken ct)
		{
			var profile = await _providerProfileRepository.GetByIdAsync(request.ProviderProfileId, ct);

			if (profile is null)
				return Result<VerifyProviderResponse>.Failure("Provider profile not found.");

			try
			{
				profile.Verify();
			}
			catch (InvalidOperationException ex)
			{
				return Result<VerifyProviderResponse>.Failure(ex.Message);
			}

			_providerProfileRepository.Update(profile);
			await _unitOfWork.SaveChangesAsync(ct);

			var response = new VerifyProviderResponse(profile.Id, profile.VerificationStatus.ToString());
			return Result<VerifyProviderResponse>.Success(response);
		}
	}
}
