using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Verification.Commands.MarkUnderReview;
 

namespace BayTack.Application.Features.Verification.Queries.GetDetail
{
	public sealed record GetVerificationDetailQuery(string Id) : IQuery<VerificationDetailResponse>;

	
}
