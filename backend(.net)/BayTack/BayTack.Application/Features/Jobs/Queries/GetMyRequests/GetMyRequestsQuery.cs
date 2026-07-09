using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Jobs.Common;

namespace BayTack.Application.Features.Jobs.Queries.GetMyRequests
{
    public sealed record GetMyRequestsQuery(string CustomerId) : IQuery<List<RequestResponse>>;
}