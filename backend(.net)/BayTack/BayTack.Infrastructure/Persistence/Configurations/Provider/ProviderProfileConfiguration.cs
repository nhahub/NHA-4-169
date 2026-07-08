using BayTack.Domain.Entities.ProviderAggregate;
using BayTack.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Configurations.Provider
{

	public class ProviderProfileConfiguration : IEntityTypeConfiguration<ProviderProfile>
	{
		public void Configure(EntityTypeBuilder<ProviderProfile> builder)
		{
			builder.Property(p => p.ProviderType).HasConversion<string>().HasMaxLength(20);
			builder.Property(p => p.VerificationStatus).HasConversion<string>().HasMaxLength(20);
			builder.Property(p => p.Bio).HasColumnType("nvarchar(max)");

			builder.OwnsOne(p => p.WorkshopAddress, a =>
			{
				a.Property(x => x.Details).HasColumnName("WorkshopAddressDetails").HasColumnType("nvarchar(max)");
				a.Property(x => x.CityId).HasColumnName("WorkshopCityId");
				a.Property(x => x.AreaId).HasColumnName("WorkshopAreaId");
			});

			builder.HasOne<AppUser>().WithOne().HasForeignKey<ProviderProfile>(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
			builder.HasIndex(p => p.UserId).IsUnique();

			builder.HasMany(p => p.Documents).WithOne().HasForeignKey(d => d.ProviderProfileId).OnDelete(DeleteBehavior.Cascade);
			builder.HasMany(p => p.Portfolio).WithOne().HasForeignKey(d => d.ProviderProfileId).OnDelete(DeleteBehavior.Cascade);
			builder.HasMany(p => p.Availabilities).WithOne().HasForeignKey(d => d.ProviderProfileId).OnDelete(DeleteBehavior.Cascade);

			builder.Metadata.FindNavigation(nameof(ProviderProfile.Documents))!.SetPropertyAccessMode(PropertyAccessMode.Field);
			builder.Metadata.FindNavigation(nameof(ProviderProfile.Portfolio))!.SetPropertyAccessMode(PropertyAccessMode.Field);
			builder.Metadata.FindNavigation(nameof(ProviderProfile.Availabilities))!.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
