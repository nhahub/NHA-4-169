using BayTack.Domain.Entities.SystemEntities;
using BayTack.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Configurations.System
{

	public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
	{
		public void Configure(EntityTypeBuilder<AuditLog> builder)
		{
			builder.Property(a => a.Action).HasMaxLength(100).IsRequired();
			builder.Property(a => a.TableName).HasMaxLength(100).IsRequired();
			builder.HasOne<AppUser>().WithMany().HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Restrict);
			builder.HasIndex(a => new { a.TableName, a.RecordId });
		}
	}
}
