using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Jobs.Common;

namespace BayTack.Application.Features.Jobs.Commands.AcceptOffer
{
    public sealed record AcceptOfferCommand(string JobId, string OfferId, string CustomerId) : ICommand<RequestResponse>;
}