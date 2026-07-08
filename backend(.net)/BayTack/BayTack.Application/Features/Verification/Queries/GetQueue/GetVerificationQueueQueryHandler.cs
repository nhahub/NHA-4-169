using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Verification.Commands.MarkUnderReview;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Verification.Queries.GetQueue
{
	public sealed class GetVerificationQueueQueryHandler
		: IQueryHandler<GetVerificationQueueQuery, IReadOnlyList<VerificationEntryResponse>>
	{
		private readonly IProviderVerificationReadRepository _repository;

		public GetVerificationQueueQueryHandler(IProviderVerificationReadRepository repository)
			=> _repository = repository;

		public async Task<Result<IReadOnlyList<VerificationEntryResponse>>> Handle(
			GetVerificationQueueQuery request, CancellationToken ct)
			=> await _repository.GetQueueAsync(request.Status, ct) is var items
				? Result<IReadOnlyList<VerificationEntryResponse>>.Success(items)
				: Result<IReadOnlyList<VerificationEntryResponse>>.Failure(" Failed to retrieve verification queue.");	
	}
}
