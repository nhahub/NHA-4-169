using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace BayTack.ReadStore.Persistence
{

	public sealed class ReadDbContext : DbContext
	{
		public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options) { }

		public DbSet<OrderReadModel> Orders => Set<OrderReadModel>();
		public DbSet<OrderHistoryReadModel> OrderHistory => Set<OrderHistoryReadModel>();
		public DbSet<NotificationReadModel> Notifications => Set<NotificationReadModel>();
		public DbSet<ServiceListingReadModel> ServiceListings => Set<ServiceListingReadModel>();
		public DbSet<ProcessedEvent> ProcessedEvents => Set<ProcessedEvent>();

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<OrderReadModel>(e =>
			{
				e.HasKey(o => o.OrderId);
				e.Property(o => o.OrderId).HasMaxLength(450);
				e.Property(o => o.FinalPriceAmount).HasColumnType("decimal(18,2)");
				e.Property(o => o.FinalPriceCurrency).HasMaxLength(3);
				e.Property(o => o.Status).HasMaxLength(20);
				e.Ignore(o => o.History); // loaded via a separate query against OrderHistory, not EF navigation

				// Indexed on the columns list/filter screens actually query by - CustomerId for
				// GetForCustomerAsync, Status for the statusGroup filter - not on FK relationships,
				// there are none here on purpose.
				e.HasIndex(o => o.CustomerId);
				e.HasIndex(o => o.Status);
			});

			builder.Entity<OrderHistoryReadModel>(e =>
			{
				e.HasKey(h => h.Id);
				e.Property(h => h.Id).ValueGeneratedOnAdd();
				e.HasIndex(h => h.OrderId);
			});

			builder.Entity<ProcessedEvent>(e =>
			{
				e.HasKey(p => p.EventId);
			});

			builder.Entity<NotificationReadModel>(e =>
			{
				e.HasKey(n => n.NotificationId);
				e.Property(n => n.NotificationId).HasMaxLength(450);
				e.Property(n => n.Type).HasMaxLength(20);
				// Indexed for the two things the notifications screen actually needs: a user's
				// own feed (UserId) and unread-count/badge queries (IsRead).
				e.HasIndex(n => n.UserId);
				e.HasIndex(n => n.IsRead);
			});

			builder.Entity<ServiceListingReadModel>(e =>
			{
				e.HasKey(s => s.ListingId);
				e.Property(s => s.ListingId).HasMaxLength(450);
				e.Property(s => s.BasicPrice).HasColumnType("decimal(18,2)");
				e.Property(s => s.StandardPrice).HasColumnType("decimal(18,2)");
				e.Property(s => s.PremiumPrice).HasColumnType("decimal(18,2)");
				// GetAllServicesQueryHandler filters by category and free-text searches Title -
				// index the former (exact match), leave the latter for SQL Server's default
				// execution plan (a LIKE '%x%' index wouldn't help much without full-text search).
				e.HasIndex(s => s.Category);
				e.HasIndex(s => s.ProviderId);
			});
		}
	}

}
