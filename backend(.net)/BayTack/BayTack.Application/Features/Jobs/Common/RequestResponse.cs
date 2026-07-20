using BayTack.Domain.Entities.JobAggregate;

namespace BayTack.Application.Features.Jobs.Common
{
    public sealed record RequestResponse(
        string Id,
        string Title,
        string Description,
        string Category,
        decimal? Budget,
        DateTime? Deadline,
        string Location,
        string Status,
        DateTime CreatedAt,
        List<OfferResponse> Offers)
    {
        // NOTE: "Category" here is actually job.ServiceId (the request is always tied to a
        // specific Service, not a free-text category name) - kept the existing field name
        // since RequestsController/frontend already key off "Category".
        public static RequestResponse FromEntity(CustomerJob job) => new(
            job.Id,
            job.Title,
            job.Description,
            job.ServiceId,
            job.Budget,
            job.Deadline,
            job.Location.Details,
            StatusFor(job.Status),
            job.CreatedAt,
            job.Bids.Select(OfferResponse.FromEntity).ToList());

        private static string StatusFor(Domain.Enums.JobStatus status) => status switch
        {
            Domain.Enums.JobStatus.Open => "open",
            Domain.Enums.JobStatus.InBidding => "open",
            Domain.Enums.JobStatus.Assigned => "assigned",
            Domain.Enums.JobStatus.InProgress => "assigned",
            Domain.Enums.JobStatus.Completed => "closed",
            Domain.Enums.JobStatus.Cancelled => "closed",
            _ => "open"
        };
    }

    public sealed record OfferResponse(string Id, string ProviderId, decimal Amount, string Currency, int DurationInDays, string? Notes, string Status)
    {
        public static OfferResponse FromEntity(ProviderBid bid) => new(
            bid.Id, bid.ProviderId, bid.ProposedPrice.Amount, bid.ProposedPrice.Currency,
            bid.DurationInDays, bid.Notes, bid.Status.ToString());
    }
}