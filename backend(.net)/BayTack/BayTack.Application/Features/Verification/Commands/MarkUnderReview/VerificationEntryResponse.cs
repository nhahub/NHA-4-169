using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Verification.Commands.MarkUnderReview
{

	public sealed record VerificationEntryResponse(
		string Id,
		string Name,
		string Category,
		string ProviderType,
		string Status,
		DateTime SubmittedAt,
		string? ImageUrl);

	public sealed record VerificationDetailResponse(
		VerificationEntryResponse Summary,
		IReadOnlyList<VerificationDocumentResponse> Documents);

	public sealed record VerificationDocumentResponse(
		string Id,
		string DocumentType,
		string Url,
		string Status);
}
