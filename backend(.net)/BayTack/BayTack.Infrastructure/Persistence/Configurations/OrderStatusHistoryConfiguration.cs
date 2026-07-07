using BayTack.Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Configurations
{

	public class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
	{
		public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
		{
			builder.Property(h => h.Status).HasConversion<string>().HasMaxLength(20);
			builder.HasOne<AppUser>().WithMany().HasForeignKey(h => h.ChangedBy).OnDelete(DeleteBehavior.Restrict);
		}
	}
}
