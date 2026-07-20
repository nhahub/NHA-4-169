using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Jobs.Common;

namespace BayTack.Application.Features.Jobs.Commands.RejectOffer
{
    public sealed record RejectOfferCommand(string JobId, string OfferId, string CustomerId) : ICommand<RequestResponse>;
}
