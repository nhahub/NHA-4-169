using BayTack.Application.Features.Verification.Commands.MarkUnderReview;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Abstractions.IRepository
{
	public interface IProviderVerificationReadRepository
	{
		Task<IReadOnlyList<VerificationEntryResponse>> GetQueueAsync(
			string? status, CancellationToken cancellationToken);

		Task<VerificationDetailResponse?> GetDetailAsync(
			string providerProfileId, CancellationToken cancellationToken);
	}
}
