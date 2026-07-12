using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Services.Common;

namespace BayTack.Application.Features.Services.Queries.GetAllServices
{
    public sealed record GetAllServicesQuery(string? Category, string? Search) : IQuery<List<ServiceResponse>>;
}