using BayTack.Domain.Entities.ServiceAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Configurations.Service
{

	public class ServiceConfiguration : IEntityTypeConfiguration<Service>
	{
		public void Configure(EntityTypeBuilder<Service> builder)
		{
			builder.Property(s => s.Name).HasMaxLength(150).IsRequired();
			builder.HasOne<ServiceCategory>().WithMany(c => c.Services).HasForeignKey(s => s.CategoryId).OnDelete(DeleteBehavior.Restrict);

			builder.OwnsOne(s => s.MinPrice, m =>
			{
				m.Property(x => x.Amount).HasColumnName("MinPrice").HasColumnType("decimal(18,2)");
				m.Property(x => x.Currency).HasColumnName("MinPriceCurrency").HasMaxLength(3);
			});
			builder.OwnsOne(s => s.MaxPrice, m =>
			{
				m.Property(x => x.Amount).HasColumnName("MaxPrice").HasColumnType("decimal(18,2)");
				m.Property(x => x.Currency).HasColumnName("MaxPriceCurrency").HasMaxLength(3);
			});

			builder.HasMany(s => s.PaymentMethods).WithOne().HasForeignKey("ServiceId").OnDelete(DeleteBehavior.Cascade);
			builder.Metadata.FindNavigation(nameof(Service.PaymentMethods))!.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}

}
