using BayTack.Domain.Entities.UserFeatures;
using BayTack.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BayTack.Infrastructure.Persistence.Configurations
{
    public class UserPreferencesConfiguration : IEntityTypeConfiguration<UserPreferences>
    {
        public void Configure(EntityTypeBuilder<UserPreferences> builder)
        {
            builder.Property(p => p.Language).HasMaxLength(10);
            builder.Property(p => p.Theme).HasMaxLength(10);

            builder.HasOne<AppUser>().WithMany().HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(p => p.UserId).IsUnique();
        }
    }
}
