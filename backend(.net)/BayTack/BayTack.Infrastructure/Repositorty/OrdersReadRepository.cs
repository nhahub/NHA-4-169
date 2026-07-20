using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Features.Orders.Common;
using BayTack.Domain.Enums;
using BayTack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BayTack.Infrastructure.Repositorty
{
    public sealed class OrdersReadRepository : IOrdersReadRepository
    {
        private readonly AppDbContext _context;

        public OrdersReadRepository(AppDbContext context) => _context = context;

        public async Task<List<OrderResponse>> GetForCustomerAsync(
            string customerId, string? statusGroup, CancellationToken ct = default)
        {
            var query =
                from o in _context.Orders.AsNoTracking()
                join cj in _context.CustomerJobs.AsNoTracking() on o.CustomerJobId equals cj.Id
                where cj.CustomerId == customerId
                select new { o, cj };

            var rows = await query
                .OrderByDescending(x => x.o.CreatedAt)
                .Select(x => new
                {
                    x.o.Id,
                    x.cj.ServiceId,
                    x.cj.Title,
                    x.o.ProviderId,
                    x.o.FinalPrice.Amount,
                    Status = x.o.Status.ToString(),
                    x.o.CreatedAt
                })
                .ToListAsync(ct);

            if (!string.IsNullOrWhiteSpace(statusGroup))
                rows = rows.Where(r => OrderResponse.StatusGroupOf(r.Status) == statusGroup.ToLowerInvariant()).ToList();

            var providerIds = rows.Select(r => r.ProviderId).Distinct().ToList();
            var providerNames = await _context.Users.AsNoTracking()
                .Where(u => providerIds.Contains(u.Id))
                .Select(u => new { u.Id, u.FullName })
                .ToDictionaryAsync(u => u.Id, u => u.FullName, ct);

            // NOTE: avatar isn't a real column on AppUser yet (same gap flagged in
            // NotificationResponse / ConversationSummaryResponse) - returning null until it exists.
            return rows.Select(r => new OrderResponse(
                r.Id,
                r.ServiceId,
                r.Title,
                providerNames.TryGetValue(r.ProviderId, out var name) ? name : "Unknown provider",
                null,
                r.Amount,
                r.Status,
                OrderResponse.ProgressFor(r.Status),
                r.CreatedAt))
                .ToList();
        }

        public async Task<OrderDetailResponse?> GetByIdForCustomerAsync(
            string customerId, string orderId, CancellationToken ct = default)
        {
            var row = await (
                from o in _context.Orders.AsNoTracking().Include(x => x.History)
                join cj in _context.CustomerJobs.AsNoTracking() on o.CustomerJobId equals cj.Id
                where o.Id == orderId && cj.CustomerId == customerId
                select new { o, cj }
            ).FirstOrDefaultAsync(ct);

            if (row is null) return null;

            var providerName = await _context.Users.AsNoTracking()
                .Where(u => u.Id == row.o.ProviderId)
                .Select(u => u.FullName)
                .FirstOrDefaultAsync(ct) ?? "Unknown provider";

            var status = row.o.Status.ToString();
            var history = row.o.History
                .OrderBy(h => h.CreatedAt)
                .Select(h => new OrderHistoryEntry(h.Status.ToString(), h.CreatedAt, h.ChangedBy))
                .ToList();

            return new OrderDetailResponse(
                row.o.Id,
                row.cj.ServiceId,
                row.cj.Title,
                row.cj.Description,
                providerName,
                null,
                row.o.FinalPrice.Amount,
                row.o.FinalPrice.Currency,
                status,
                OrderResponse.ProgressFor(status),
                row.o.CreatedAt,
                row.o.StartDate,
                row.o.EndDate,
                history);
        }

        public async Task<string?> GetProviderNameAsync(string providerId, CancellationToken ct = default) =>
            await _context.Users.AsNoTracking()
                .Where(u => u.Id == providerId)
                .Select(u => u.FullName)
                .FirstOrDefaultAsync(ct);
    }
}