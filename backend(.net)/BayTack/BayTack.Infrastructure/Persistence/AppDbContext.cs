using BayTack.Domain.Common.BaseEntity;
using BayTack.Domain.Entities;
using BayTack.Domain.Entities.JobAggregate;
using BayTack.Domain.Entities.Location;
using BayTack.Domain.Entities.Messaging;
using BayTack.Domain.Entities.OrderAggregate;
using BayTack.Domain.Entities.PaymentAggregate;
using BayTack.Domain.Entities.ProviderAggregate;
using BayTack.Domain.Entities.ServiceAggregate;
using BayTack.Domain.Entities.SystemEntities;
using BayTack.Domain.Entities.UserFeatures;
using BayTack.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace BayTack.Infrastructure.Persistence
{
	public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<string>, string>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<City> Cities => Set<City>();
		public DbSet<Area> Areas => Set<Area>();

		public DbSet<ProviderProfile> ProviderProfiles => Set<ProviderProfile>();
		public DbSet<ProviderDocument> ProviderDocuments => Set<ProviderDocument>();
		public DbSet<ProviderPortfolioItem> ProviderPortfolioItems => Set<ProviderPortfolioItem>();
		public DbSet<ProviderAvailability> ProviderAvailabilities => Set<ProviderAvailability>();

		public DbSet<ServiceCategory> ServiceCategories => Set<ServiceCategory>();
		public DbSet<Service> Services => Set<Service>();

		public DbSet<CustomerJob> CustomerJobs => Set<CustomerJob>();
		public DbSet<JobImage> JobImages => Set<JobImage>();
		public DbSet<ProviderBid> ProviderBids => Set<ProviderBid>();

		public DbSet<Order> Orders => Set<Order>();
		public DbSet<OrderStatusHistory> OrderStatusHistories => Set<OrderStatusHistory>();

		public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
		public DbSet<ServicePaymentMethod> ServicePaymentMethods => Set<ServicePaymentMethod>();
		public DbSet<Payment> Payments => Set<Payment>();

		public DbSet<Review> Reviews => Set<Review>();

		public DbSet<Favorite> Favorites => Set<Favorite>();
		public DbSet<Notification> Notifications => Set<Notification>();

		public DbSet<Attachment> Attachments => Set<Attachment>();
		public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

       // public object Conversations { get; internal set; }
        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<Message> Messages => Set<Message>();


		public DbSet<RefreshToken> RefreshTokens { get; set; }




		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

			// Applies a global "WHERE IsDeleted = 0" filter to every entity that
			// implements ISoftDelete, instead of relying on every query to remember it.
			foreach (var entityType in builder.Model.GetEntityTypes())
			{
				if (!typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType)) continue;

				var parameter = Expression.Parameter(entityType.ClrType, "e");
				var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
				var condition = Expression.Equal(property, Expression.Constant(false));
				var lambda = Expression.Lambda(condition, parameter);
				builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
			}





				builder.Entity<RefreshToken>()
				.HasOne(rt => rt.AppUser)
				.WithMany(u => u.RefreshTokens)
				.HasForeignKey(rt => rt.UserId)
				.IsRequired(false); 
		}
	}
}