using BayTack.Domain.Entities.SystemEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BayTack.Infrastructure.Persistence.Configurations.System
{
	public class PlatformSettingsConfiguration : IEntityTypeConfiguration<PlatformSettings>
	{
		public void Configure(EntityTypeBuilder<PlatformSettings> builder)
		{
			builder.Property(s => s.PlatformFee).HasColumnType("decimal(18,2)");
			builder.Property(s => s.DefaultUserRole).HasMaxLength(50).IsRequired();
			builder.Property(s => s.SupportEmail).HasMaxLength(200).IsRequired();
			builder.Property(s => s.MaintenanceMessage).HasColumnType("nvarchar(max)");
		}
	}
}
