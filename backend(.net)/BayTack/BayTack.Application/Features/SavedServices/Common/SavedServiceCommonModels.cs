using BayTack.Domain.Entities.ServiceAggregate;

namespace BayTack.Application.Features.SavedServices.Common
{
    /// <summary>Shape expected by Front_end/customer/app/saved (was mocked -> bt_c_saved as an id array;
    /// the endpoint returns full Service objects for those ids).</summary>
    public sealed record ServiceResponse(
        string Id,
        string CategoryId,
        string Name,
        string? Description,
        decimal MinPrice,
        decimal MaxPrice,
        string Currency,
        bool AllowCredit,
        bool AllowInstallments)
    {
        public static ServiceResponse FromEntity(Service service) => new(
            service.Id,
            service.CategoryId,
            service.Name,
            service.Description,
            service.MinPrice.Amount,
            service.MaxPrice.Amount,
            service.MinPrice.Currency,
            service.AllowCredit,
            service.AllowInstallments);
    }
}