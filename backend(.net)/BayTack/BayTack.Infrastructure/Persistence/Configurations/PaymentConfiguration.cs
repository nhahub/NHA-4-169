using BayTack.Domain.Entities.OrderAggregate;
using BayTack.Domain.Entities.PaymentAggregate;
using BayTack.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Configurations
{

	public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
	{
		public void Configure(EntityTypeBuilder<Payment> builder)
		{
			builder.HasOne<Order>().WithMany().HasForeignKey(p => p.OrderId).OnDelete(DeleteBehavior.Restrict);
			builder.HasOne<PaymentMethod>().WithMany().HasForeignKey(p => p.MethodId).OnDelete(DeleteBehavior.Restrict);
			builder.HasOne<AppUser>().WithMany().HasForeignKey(p => p.AdminId).OnDelete(DeleteBehavior.Restrict);

			builder.OwnsOne(p => p.Amount, m =>
			{
				m.Property(x => x.Amount).HasColumnName("Amount").HasColumnType("decimal(18,2)");
				m.Property(x => x.Currency).HasColumnName("AmountCurrency").HasMaxLength(3);
			});
			builder.OwnsOne(p => p.OurCommission, m =>
			{
				m.Property(x => x.Amount).HasColumnName("OurCommission").HasColumnType("decimal(18,2)");
				m.Property(x => x.Currency).HasColumnName("CommissionCurrency").HasMaxLength(3);
			});
			builder.OwnsOne(p => p.ProviderReceived, m =>
			{
				m.Property(x => x.Amount).HasColumnName("ProviderReceived").HasColumnType("decimal(18,2)");
				m.Property(x => x.Currency).HasColumnName("ProviderReceivedCurrency").HasMaxLength(3);
			});
		}
	}

}
