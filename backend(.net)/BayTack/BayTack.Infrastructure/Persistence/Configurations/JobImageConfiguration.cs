using BayTack.Domain.Entities.JobAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Configurations
{
	public class JobImageConfiguration : IEntityTypeConfiguration<JobImage>
	{
		public void Configure(EntityTypeBuilder<JobImage> builder)
		{
			builder.Property(i => i.ImageUrl).HasColumnType("nvarchar(max)").IsRequired();
		}
	}
}
