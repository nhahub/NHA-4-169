using BayTack.Domain.Entities.SystemEntities;
using BayTack.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Configurations.System
{

	public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
	{
		public void Configure(EntityTypeBuilder<Attachment> builder)
		{
			builder.Property(a => a.FileUrl).HasColumnType("nvarchar(max)").IsRequired();
			builder.Property(a => a.FileType).HasMaxLength(50).IsRequired();
			builder.HasOne<AppUser>().WithMany().HasForeignKey(a => a.UploadedBy).OnDelete(DeleteBehavior.Restrict);
		}
	}
}
