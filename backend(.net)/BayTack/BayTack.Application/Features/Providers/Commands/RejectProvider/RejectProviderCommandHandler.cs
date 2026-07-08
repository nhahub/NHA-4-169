using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Providers.Commands.RejectProvider
{
	public sealed class RejectProviderCommandHandler
		: ICommandHandler<RejectProviderCommand, RejectProviderResponse>
	{
		private readonly IRepository<ProviderProfile, string> _providerProfileRepository;
		private readonly IUnitOfWork _unitOfWork;

		public RejectProviderCommandHandler(
			IRepository<ProviderProfile, string> providerProfileRepository,
			IUnitOfWork unitOfWork)
		{
			_providerProfileRepository = providerProfileRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<RejectProviderResponse>> Handle(
			RejectProviderCommand request, CancellationToken ct)
		{
			var profile = await _providerProfileRepository.GetByIdAsync(request.ProviderProfileId, ct);

			if (profile is null)
				return Result<RejectProviderResponse>.Failure("Provider profile not found.");

			profile.Reject();

			_providerProfileRepository.Update(profile);
			await _unitOfWork.SaveChangesAsync(ct);

			var response = new RejectProviderResponse(profile.Id, profile.VerificationStatus.ToString());
			return Result<RejectProviderResponse>.Success(response);
		}
	}
}
