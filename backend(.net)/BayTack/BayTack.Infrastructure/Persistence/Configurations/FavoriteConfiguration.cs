using BayTack.Domain.Entities.UserFeatures;
using BayTack.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Configurations
{

	public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
	{
		public void Configure(EntityTypeBuilder<Favorite> builder)
		{
			builder.HasOne<AppUser>().WithMany().HasForeignKey(f => f.CustomerId).OnDelete(DeleteBehavior.Restrict);
			builder.HasOne<AppUser>().WithMany().HasForeignKey(f => f.ProviderId).OnDelete(DeleteBehavior.Restrict);
			builder.HasIndex(f => new { f.CustomerId, f.ProviderId }).IsUnique();
		}
	}
}
