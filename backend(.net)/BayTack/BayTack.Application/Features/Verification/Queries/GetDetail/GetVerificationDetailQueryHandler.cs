using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Verification.Commands.MarkUnderReview;


namespace BayTack.Application.Features.Verification.Queries.GetDetail
{
	public sealed class GetVerificationDetailQueryHandler
		: IQueryHandler<GetVerificationDetailQuery, VerificationDetailResponse>
	{
		private readonly IProviderVerificationReadRepository _repository;

		public GetVerificationDetailQueryHandler(IProviderVerificationReadRepository repository)
			=> _repository = repository;

		public async Task<Result<VerificationDetailResponse>> Handle(
			GetVerificationDetailQuery request, CancellationToken ct)
		{
			var detail = await _repository.GetDetailAsync(request.Id, ct);
			return detail is not null ? Result<VerificationDetailResponse>.Success(detail) : Result<VerificationDetailResponse>.Failure(" ");
		}
	}
}
