using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Features.Orders.Common;
using BayTack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using BayTack.ReadStore.Persistence;


namespace BayTack.Infrastructure.Repositorty
{
	public sealed class OrdersReadRepository : IOrdersReadRepository
	{
		private readonly AppDbContext _appDb;
		private readonly ReadDbContext _readDb;

		public OrdersReadRepository(ReadDbContext readDb, AppDbContext appDb)
		{
			_readDb = readDb;
			_appDb = appDb;
		}

		public async Task<List<OrderResponse>> GetForCustomerAsync(
			string customerId, string? statusGroup, CancellationToken ct = default)
		{
			var rows = await _readDb.Orders.AsNoTracking()
				.Where(o => o.CustomerId == customerId)
				.OrderByDescending(o => o.CreatedAtUtc)
				.ToListAsync(ct);

			if (!string.IsNullOrWhiteSpace(statusGroup))
				rows = rows.Where(r => OrderResponse.StatusGroupOf(r.Status) == statusGroup.ToLowerInvariant()).ToList();

			return rows.Select(r => new OrderResponse(
				r.OrderId,
				r.ServiceId,
				r.Title,
				r.ProviderName,
				null, // avatar - same gap as the Write-side implementation had; not a real column yet
				r.FinalPriceAmount,
				r.Status,
				OrderResponse.ProgressFor(r.Status),
				r.CreatedAtUtc))
				.ToList();
		}

		public async Task<OrderDetailResponse?> GetByIdForCustomerAsync(
			string customerId, string orderId, CancellationToken ct = default)
		{
			var order = await _readDb.Orders.AsNoTracking()
				.FirstOrDefaultAsync(o => o.OrderId == orderId && o.CustomerId == customerId, ct);

			if (order is null) return null;

			var history = await _readDb.OrderHistory.AsNoTracking()
				.Where(h => h.OrderId == orderId)
				.OrderBy(h => h.ChangedAtUtc)
				.Select(h => new OrderHistoryEntry(h.Status, h.ChangedAtUtc, h.ChangedBy))
				.ToListAsync(ct);

			return new OrderDetailResponse(
				order.OrderId,
				order.ServiceId,
				order.Title,
				order.Description,
				order.ProviderName,
				null,
				order.FinalPriceAmount,
				order.FinalPriceCurrency,
				order.Status,
				OrderResponse.ProgressFor(order.Status),
				order.CreatedAtUtc,
				order.StartDate,
				order.EndDate,
				history);
		}



		/// <summary>Still used by command handlers assembling a response right after a
		/// mutation (see CreateOrderCommandHandler notes) - a fresh read-side query would
		/// miss the not-yet-projected change, same reasoning as before, just against the
		/// read model's ProviderName column instead of a live join to Users.</summary>
		public async Task<string?> GetProviderNameAsync(string providerId, CancellationToken ct = default) =>
			await _readDb.Orders.AsNoTracking()
				.Where(o => o.ProviderId == providerId)
				.Select(o => o.ProviderName)
				.FirstOrDefaultAsync(ct);

	}
}