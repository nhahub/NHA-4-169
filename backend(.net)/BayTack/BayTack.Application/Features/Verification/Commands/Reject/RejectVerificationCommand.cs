using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Verification.Commands.MarkUnderReview;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Application.Features.Verification.Commands.Reject
{
	public sealed record RejectVerificationCommand(string ProviderProfileId, string Reason) : ICommand<VerificationEntryResponse>;

}
