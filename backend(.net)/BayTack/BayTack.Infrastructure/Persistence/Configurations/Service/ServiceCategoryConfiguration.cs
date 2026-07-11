using BayTack.Domain.Entities.ServiceAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Configurations.Service
{

	public class ServiceCategoryConfiguration : IEntityTypeConfiguration<ServiceCategory>
	{
		public void Configure(EntityTypeBuilder<ServiceCategory> builder)
		{
			builder.Property(c => c.Name).HasMaxLength(150).IsRequired();
			builder.HasIndex(c => c.Name).IsUnique();
			builder.Property(c => c.Icon).HasMaxLength(100);
		}
	}
}
