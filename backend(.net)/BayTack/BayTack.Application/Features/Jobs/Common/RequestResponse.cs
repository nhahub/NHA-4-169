using BayTack.Domain.Entities.JobAggregate;

namespace BayTack.Application.Features.Jobs.Common
{
    /// <summary>Shape expected by Front_end/customer/app/post-request, requests/ (was mocked -> bt_c_requests).</summary>
    public sealed record RequestResponse(
        string Id,
        string Title,
        string Category,
        decimal? Budget,
        DateTime? Deadline,
        string Location,
        string Status,
        List<OfferResponse> Offers)
    {
        // CustomerJob doesn't persist Budget/Deadline yet (they aren't in the original
        // domain model - see Domain/Entities/JobAggregate/CustomerJob.cs). Returning null
        // rather than guessing a value. If the frontend needs these, they should become
        // real columns on CustomerJob (with a migration) instead of being faked here.
        public static RequestResponse FromEntity(CustomerJob job) => new(
            job.Id,
            job.Title,
            job.ServiceId,
            null,
            null,
            job.Location.Details,
            StatusFor(job.Status),
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