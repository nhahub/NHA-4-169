using BayTack.Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Configurations
{

	public class CityConfiguration : IEntityTypeConfiguration<City>
	{
		public void Configure(EntityTypeBuilder<City> builder)
		{
			builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
			builder.HasIndex(c => c.Name).IsUnique();
			builder.Metadata.FindNavigation(nameof(City.Areas))!
				.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
