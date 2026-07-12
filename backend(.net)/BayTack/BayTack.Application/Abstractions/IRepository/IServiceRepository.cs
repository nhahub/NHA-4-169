using BayTack.Application.Features.Services.Common;

namespace BayTack.Application.Abstractions.IRepository
{
    /// <summary>
    /// Generic IRepository&lt;Service, string&gt; isn't enough here because the response
    /// needs: ServiceCategory.Name (join), AppUser.FullName for the provider (join), and a
    /// live average of Review.Rating for that provider's completed orders (join through
    /// Order) - three aggregates a single Specification over Service can't reach. Same
    /// reasoning as IConversationRepository/IOrderRepository.
    /// </summary>
    public interface IServiceRepository
    {
        Task<List<ServiceResponse>> SearchAsync(
            string? category, string? search, CancellationToken ct = default);

        Task<ServiceResponse?> GetByIdAsync(string serviceId, CancellationToken ct = default);
    }
}