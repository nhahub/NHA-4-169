using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Features.Jobs.Common;

namespace BayTack.Application.Features.Jobs.Commands.CreateRequest
{
    // NOTE: Budget/Deadline are accepted from the client but not persisted yet - CustomerJob
    // has no columns for them (see RequestResponse.FromEntity). They're kept on the command
    // so the validator/API shape matches the frontend today; wire them up once the columns exist.
    public sealed record CreateRequestCommand(
        string CustomerId,
        string ServiceId,
        string Title,
        string Description,
        string LocationDetails,
        int CityId,
        int? AreaId,
        decimal? Budget,
        DateTime? Deadline,
        string? PreferredPayment) : ICommand<RequestResponse>;
}