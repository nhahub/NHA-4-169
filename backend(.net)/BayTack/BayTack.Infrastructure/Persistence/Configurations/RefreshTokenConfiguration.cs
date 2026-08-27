using BayTack.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BayTack.Infrastructure.Persistence.Configurations
{
	public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
	{
		public void Configure(EntityTypeBuilder<RefreshToken> builder)
		{
			builder.HasOne(rt => rt.AppUser)
				   .WithMany(u => u.RefreshTokens)
				   .HasForeignKey(rt => rt.UserId)
				   .IsRequired(false);
		}
	}
}