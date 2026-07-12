using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Services.Common;

namespace BayTack.Application.Features.Services.Queries.GetServiceById
{
    public sealed record GetServiceByIdQuery(string ServiceId) : IQuery<ServiceResponse>;
}