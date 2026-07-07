using BayTack.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Configurations
{
	public class ApplicationUserConfiguration : IEntityTypeConfiguration<AppUser>
	{
		public void Configure(EntityTypeBuilder<AppUser> builder)
		{
			builder.Property(u => u.FullName).HasMaxLength(150).IsRequired();
			builder.Property(u => u.Status).HasConversion<string>().HasMaxLength(20);
			builder.Property(u => u.DeleteReason).HasColumnType("nvarchar(max)");

			builder.OwnsOne(u => u.Address, a =>
			{
				a.Property(x => x.Details).HasColumnName("AddressDetails").HasColumnType("nvarchar(max)");
				a.Property(x => x.CityId).HasColumnName("AddressCityId");
				a.Property(x => x.AreaId).HasColumnName("AddressAreaId");
			});

			builder.HasOne<AppUser>().WithMany().HasForeignKey(u => u.UpdatedBy).OnDelete(DeleteBehavior.Restrict);
			builder.HasOne<AppUser>().WithMany().HasForeignKey(u => u.DeletedBy).OnDelete(DeleteBehavior.Restrict);

			builder.HasQueryFilter(u => !u.IsDeleted);
		}
	}
}
