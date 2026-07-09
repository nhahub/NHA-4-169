using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Jobs.Common;

namespace BayTack.Application.Features.Jobs.Queries.GetRequestById
{
    public sealed record GetRequestByIdQuery(string JobId, string CustomerId) : IQuery<RequestResponse>;
}