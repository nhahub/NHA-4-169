using BayTack.Domain.Entities.ProviderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Configurations.Provider
{

	public class ProviderAvailabilityConfiguration : IEntityTypeConfiguration<ProviderAvailability>
	{
		public void Configure(EntityTypeBuilder<ProviderAvailability> builder)
		{
			builder.Property(a => a.DayOfWeek).HasConversion<string>().HasMaxLength(20);
			builder.HasIndex(a => new { a.ProviderProfileId, a.DayOfWeek }).IsUnique();
		}
	}

}
