using BayTack.Domain.Entities.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Configurations
{

	public class AreaConfiguration : IEntityTypeConfiguration<Area>
	{
		public void Configure(EntityTypeBuilder<Area> builder)
		{
			builder.Property(a => a.Name).HasMaxLength(100).IsRequired();
			builder.HasOne<City>().WithMany(c => c.Areas).HasForeignKey(a => a.CityId).OnDelete(DeleteBehavior.Cascade);
			builder.HasIndex(a => new { a.CityId, a.Name }).IsUnique();
		}
	}

}
