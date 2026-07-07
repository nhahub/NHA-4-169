using BayTack.Domain.Entities.JobAggregate;
using BayTack.Domain.Entities.OrderAggregate;
using BayTack.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Configurations
{

	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(20);

			builder.OwnsOne(o => o.FinalPrice, m =>
			{
				m.Property(x => x.Amount).HasColumnName("FinalPrice").HasColumnType("decimal(18,2)");
				m.Property(x => x.Currency).HasColumnName("FinalPriceCurrency").HasMaxLength(3);
			});

			builder.HasOne<CustomerJob>().WithMany().HasForeignKey(o => o.CustomerJobId).OnDelete(DeleteBehavior.Restrict);
			builder.HasOne<AppUser>().WithMany().HasForeignKey(o => o.ProviderId).OnDelete(DeleteBehavior.Restrict);

			builder.HasMany(o => o.History).WithOne().HasForeignKey(h => h.OrderId).OnDelete(DeleteBehavior.Cascade);
			builder.Metadata.FindNavigation(nameof(Order.History))!.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
