using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ProviderAggregate;

namespace BayTack.Application.Features.Verification.Commands.MarkUnderReview
{

	public sealed class MarkUnderReviewCommandHandler
		: ICommandHandler<MarkUnderReviewCommand, VerificationEntryResponse>
	{
		private readonly IRepository<ProviderProfile, string> _providerRepository;
		private readonly IProviderVerificationReadRepository _readRepository;

		public MarkUnderReviewCommandHandler(
			IRepository<ProviderProfile, string> providerRepository,
			IProviderVerificationReadRepository readRepository)
		{
			_providerRepository = providerRepository;
			_readRepository = readRepository;
		}

		public async Task<Result<VerificationEntryResponse>> Handle(
			MarkUnderReviewCommand request, CancellationToken ct)
		{
			var profile = await _providerRepository.FirstOrDefaultAsync(
				new ProviderProfileByIdSpec(request.ProviderProfileId, forUpdate: true), ct);

			if (profile is null) return Result<VerificationEntryResponse>.Failure("Provider profile not found.");

			profile.MarkUnderReview();
			_providerRepository.Update(profile);

			var detail = await _readRepository.GetDetailAsync(request.ProviderProfileId, ct);
			return Result<VerificationEntryResponse>.Success(detail!.Summary);

			// Status here reflects pre-commit state via domain method result naming, not a re-read after save —
			// UnitOfWorkBehavior saves after this returns.
		}

	}
}