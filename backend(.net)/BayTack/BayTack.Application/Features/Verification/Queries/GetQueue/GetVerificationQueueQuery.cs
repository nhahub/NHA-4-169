using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Verification.Commands.MarkUnderReview;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Verification.Queries.GetQueue
{
	public sealed record GetVerificationQueueQuery(string? Status) : IQuery<IReadOnlyList<VerificationEntryResponse>>;

	
}
