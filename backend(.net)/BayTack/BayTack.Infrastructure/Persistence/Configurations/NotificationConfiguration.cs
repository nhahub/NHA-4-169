using BayTack.Domain.Entities.UserFeatures;
using BayTack.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BayTack.Infrastructure.Persistence.Configurations
{

	public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
	{
		public void Configure(EntityTypeBuilder<Notification> builder)
		{
			builder.Property(n => n.Title).HasMaxLength(200).IsRequired();
			builder.Property(n => n.Message).HasColumnType("nvarchar(max)").IsRequired();
			builder.Property(n => n.Type).HasConversion<string>().HasMaxLength(20);
			builder.HasOne<AppUser>().WithMany().HasForeignKey(n => n.UserId).OnDelete(DeleteBehavior.Cascade);
			builder.HasIndex(n => new { n.UserId, n.IsRead });
		}
	}
}
