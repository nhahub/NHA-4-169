using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Features.Services.Common;
using BayTack.Domain.Entities;
using BayTack.Domain.Entities.OrderAggregate;
using BayTack.Domain.Entities.ServiceAggregate;
using BayTack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BayTack.Infrastructure.Repositorty
{
    public sealed class ServiceRepository : IServiceRepository
    {
        private readonly AppDbContext _context;

        public ServiceRepository(AppDbContext context) => _context = context;

        // "id" in ServiceResponse is the ServiceListing.Id - that's what a customer
        // actually browses/books, since it's the specific provider+pricing combination.
        public async Task<List<ServiceResponse>> SearchAsync(
            string? category, string? search, CancellationToken ct = default)
        {
            var query = _context.ServiceListings.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(l => _context.Services
                    .Any(s => s.Id == l.ServiceId &&
                        _context.Set<ServiceCategory>().Any(c => c.Id == s.CategoryId && c.Name.Contains(category))));
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(l => _context.Services.Any(s => s.Id == l.ServiceId &&
                    (s.Name.Contains(search) || (s.Description != null && s.Description.Contains(search)))));
            }

            var rows = await Project(query).ToListAsync(ct);
            return rows.Select(Map).ToList();
        }

        public async Task<ServiceResponse?> GetByIdAsync(string listingId, CancellationToken ct = default)
        {
            var query = _context.ServiceListings.AsNoTracking().Where(l => l.Id == listingId);
            var row = await Project(query).FirstOrDefaultAsync(ct);
            return row is null ? null : Map(row);
        }

        // Shared projection so GetAll/GetById compute rating and tiers the exact same way.
        private IQueryable<Row> Project(IQueryable<ServiceListing> query) => query.Select(l => new Row(
            l.Id,
            l.ServiceId,
            _context.Services.Where(s => s.Id == l.ServiceId).Select(s => s.Name).FirstOrDefault() ?? "",
            _context.Services.Where(s => s.Id == l.ServiceId)
                .Select(s => _context.Set<ServiceCategory>().Where(c => c.Id == s.CategoryId).Select(c => c.Name).FirstOrDefault())
                .FirstOrDefault() ?? "",
            l.IconName,
            l.ProviderId,
            _context.Users.Where(u => u.Id == l.ProviderId).Select(u => u.FullName).FirstOrDefault() ?? "Unknown provider",
            // Live average of Review.Rating for orders completed by this provider - no
            // stored rating column anywhere yet, computed on read as agreed.
            _context.Set<Review>()
                .Where(r => _context.Set<Order>().Any(o => o.Id == r.OrderId && o.ProviderId == l.ProviderId))
                .Select(r => (double?)r.Rating)
                .Average(),
            l.BasicPackage.Name, l.BasicPackage.Price.Amount, l.BasicPackage.Description, l.BasicPackage.DeliveryEstimate,
            l.StandardPackage.Name, l.StandardPackage.Price.Amount, l.StandardPackage.Description, l.StandardPackage.DeliveryEstimate,
            l.PremiumPackage.Name, l.PremiumPackage.Price.Amount, l.PremiumPackage.Description, l.PremiumPackage.DeliveryEstimate));

        private static ServiceResponse Map(Row r) => new(
            r.Id, r.Title, r.Category, r.Icon, r.Provider, r.ProviderId,
            null, // avatar: AppUser has no avatar column yet - same gap flagged elsewhere.
            Math.Round(r.Rating ?? 0, 1),
            new TiersResponse(
                new TierResponse(r.BasicName, r.BasicPrice, r.BasicDescription, r.BasicDelivery),
                new TierResponse(r.StandardName, r.StandardPrice, r.StandardDescription, r.StandardDelivery),
                new TierResponse(r.PremiumName, r.PremiumPrice, r.PremiumDescription, r.PremiumDelivery)));

        private sealed record Row(
            string Id, string ServiceId, string Title, string Category, string? Icon, string ProviderId, string Provider, double? Rating,
            string BasicName, decimal BasicPrice, string BasicDescription, string BasicDelivery,
            string StandardName, decimal StandardPrice, string StandardDescription, string StandardDelivery,
            string PremiumName, decimal PremiumPrice, string PremiumDescription, string PremiumDelivery);
    }
}