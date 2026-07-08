using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Verification.Commands.MarkUnderReview;
using BayTack.Domain.Entities.ProviderAggregate;


namespace BayTack.Application.Features.Verification.Commands.Reject
{
	public sealed class RejectVerificationCommandHandler
		: ICommandHandler<RejectVerificationCommand, VerificationEntryResponse>
	{
		private readonly IRepository<ProviderProfile, string> _providerRepository;
		private readonly IProviderVerificationReadRepository _readRepository;

		public RejectVerificationCommandHandler(
			IRepository<ProviderProfile, string> providerRepository,
			IProviderVerificationReadRepository readRepository)
		{
			_providerRepository = providerRepository;
			_readRepository = readRepository;
		}

		public async Task<Result<VerificationEntryResponse>> Handle(
			RejectVerificationCommand request, CancellationToken ct)
		{
			var profile = await _providerRepository.FirstOrDefaultAsync(
				new ProviderProfileByIdSpec(request.ProviderProfileId, forUpdate: true), ct);

			if (profile is null) return Result<VerificationEntryResponse>.Failure("Provider profile not found.");

			profile.Reject(request.Reason);
			_providerRepository.Update(profile);

			var detail = await _readRepository.GetDetailAsync(request.ProviderProfileId, ct);
			return Result<VerificationEntryResponse>.Success(detail!.Summary);
		}
	}
}
