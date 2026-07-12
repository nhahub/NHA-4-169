using System.Text.Json.Serialization;

namespace BayTack.Application.Features.Services.Common
{
    /// <summary>Field names use [JsonPropertyName] where they differ from our normal C#
    /// naming (desc/delivery) because this mirrors the exact shape Front_end's mock
    /// (bt_c_services) already uses - see customer-data.js tiers.basic/standard/premium.</summary>
    public sealed record TierResponse(
        string Name,
        decimal Price,
        [property: JsonPropertyName("desc")] string Description,
        [property: JsonPropertyName("delivery")] string DeliveryEstimate);

    public sealed record TiersResponse(TierResponse? Basic, TierResponse? Standard, TierResponse? Premium);

    /// <summary>Shape expected by Front_end/customer/app/browse, service/, saved/
    /// (currently mocked -> bt_c_services): { id, title, category, icon, provider,
    /// providerId, avatar, rating, tiers }.</summary>
    public sealed record ServiceResponse(
        string Id,
        string Title,
        string Category,
        string? Icon,
        string Provider,
        string ProviderId,
        string? Avatar,
        double Rating,
        TiersResponse Tiers);
}