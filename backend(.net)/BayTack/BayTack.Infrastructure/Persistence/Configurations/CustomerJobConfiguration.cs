using BayTack.Domain.Entities.JobAggregate;
using BayTack.Domain.Entities.ServiceAggregate;
using BayTack.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using entityService = BayTack.Domain.Entities.ServiceAggregate.Service;


namespace BayTack.Infrastructure.Persistence.Configurations
{
	public class CustomerJobConfiguration : IEntityTypeConfiguration<CustomerJob>
	{
		public void Configure(EntityTypeBuilder<CustomerJob> builder)
		{
			builder.Property(j => j.Title).HasMaxLength(150).IsRequired();
			builder.Property(j => j.Description).HasColumnType("nvarchar(max)").IsRequired();
			builder.Property(j => j.PreferredPayment).HasMaxLength(50);
			builder.Property(j => j.Budget).HasColumnType("decimal(18,2)");
			builder.Property(j => j.Status).HasConversion<string>().HasMaxLength(20);
			builder.Property(j => j.DeleteReason).HasColumnType("nvarchar(max)");

			builder.OwnsOne(j => j.Location, a =>
			{
				a.Property(x => x.Details).HasColumnName("LocationDetails").HasColumnType("nvarchar(max)").IsRequired();
				a.Property(x => x.CityId).HasColumnName("LocationCityId").HasMaxLength(450).IsRequired();
				a.Property(x => x.AreaId).HasColumnName("LocationAreaId").HasMaxLength(450);
			});

			builder.HasOne<AppUser>().WithMany().HasForeignKey(j => j.CustomerId).OnDelete(DeleteBehavior.Restrict);
			builder.HasOne<entityService>().WithMany().HasForeignKey(j => j.ServiceId).OnDelete(DeleteBehavior.Restrict);
			builder.HasOne<AppUser>().WithMany().HasForeignKey(j => j.DeletedBy).OnDelete(DeleteBehavior.Restrict);

			builder.HasMany(j => j.Images).WithOne().HasForeignKey(i => i.CustomerJobId).OnDelete(DeleteBehavior.Cascade);
			builder.HasMany(j => j.Bids).WithOne().HasForeignKey(b => b.CustomerJobId).OnDelete(DeleteBehavior.Cascade);

			builder.Metadata.FindNavigation(nameof(CustomerJob.Images))!.SetPropertyAccessMode(PropertyAccessMode.Field);
			builder.Metadata.FindNavigation(nameof(CustomerJob.Bids))!.SetPropertyAccessMode(PropertyAccessMode.Field);

			builder.HasQueryFilter(j => !j.IsDeleted);
		}
	}

	
}
