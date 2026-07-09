using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.SavedServices.Common;

namespace BayTack.Application.Features.SavedServices.Queries.GetSavedServices
{
    public sealed record GetSavedServicesQuery(string CustomerId) : IQuery<List<ServiceResponse>>;
}