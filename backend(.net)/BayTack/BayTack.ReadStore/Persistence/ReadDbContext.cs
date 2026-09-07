using BayTack.ReadStore.Models;
using Microsoft.EntityFrameworkCore;

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
				e.Property(o => o.FinalPriceAmount).HasPrecision(18, 2);
				e.Property(o => o.FinalPriceCurrency).HasMaxLength(3);
				e.Property(o => o.Status).HasMaxLength(20);
				e.Property(o => o.CustomerId).HasMaxLength(450);
				e.Ignore(o => o.History); // loaded via a separate query against OrderHistory, not EF navigation
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
				e.HasIndex(n => n.UserId);
				e.HasIndex(n => n.IsRead);
			});

			builder.Entity<ServiceListingReadModel>(e =>
			{
				e.HasKey(s => s.ListingId);
				e.Property(s => s.ListingId).HasMaxLength(450);
				e.Property(s => s.BasicPrice).HasPrecision(18, 2);
				e.Property(s => s.StandardPrice).HasPrecision(18, 2);
				e.Property(s => s.PremiumPrice).HasPrecision(18, 2);
				e.HasIndex(s => s.Category);
				e.HasIndex(s => s.ProviderId);
			});
		}
	}

}
