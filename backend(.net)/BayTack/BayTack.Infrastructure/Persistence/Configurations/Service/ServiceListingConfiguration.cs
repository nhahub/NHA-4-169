using BayTack.Domain.Entities.ServiceAggregate;
using BayTack.Domain.ValueObjects;
using BayTack.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;

namespace BayTack.Infrastructure.Persistence.Configurations.Service
{
    public class ServiceListingConfiguration : IEntityTypeConfiguration<ServiceListing>
    {
        public void Configure(EntityTypeBuilder<ServiceListing> builder)
        {
            builder.HasOne<BayTack.Domain.Entities.ServiceAggregate.Service>()
                .WithMany().HasForeignKey(l => l.ServiceId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<AppUser>().WithMany().HasForeignKey(l => l.ProviderId).OnDelete(DeleteBehavior.Restrict);

            // One listing per provider per catalog service - a provider doesn't offer the
            // same service twice.
            builder.HasIndex(l => new { l.ServiceId, l.ProviderId }).IsUnique();
            builder.HasIndex(l => l.ProviderId);

            ConfigurePackage(builder, l => l.BasicPackage, "Basic");
            ConfigurePackage(builder, l => l.StandardPackage, "Standard");
            ConfigurePackage(builder, l => l.PremiumPackage, "Premium");
        }

        private static void ConfigurePackage(
            EntityTypeBuilder<ServiceListing> builder,
            Expression<Func<ServiceListing, ServicePackage>> selector,
            string columnPrefix)
        {
            builder.OwnsOne(selector, p =>
            {
                p.Property(x => x.Name).HasColumnName($"{columnPrefix}Name").HasMaxLength(100).IsRequired();
                p.Property(x => x.Description).HasColumnName($"{columnPrefix}Description").HasMaxLength(500);
                p.Property(x => x.DeliveryEstimate).HasColumnName($"{columnPrefix}Delivery").HasMaxLength(50);
                p.OwnsOne(x => x.Price, m =>
                {
                    m.Property(x => x.Amount).HasColumnName($"{columnPrefix}Price").HasColumnType("decimal(18,2)");
                    m.Property(x => x.Currency).HasColumnName($"{columnPrefix}PriceCurrency").HasMaxLength(3);
                });
            });
        }
    }
}