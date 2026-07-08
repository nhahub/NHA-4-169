using BayTack.Domain.Entities.JobAggregate;
using BayTack.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BayTack.Infrastructure.Persistence.Configurations
{

	public class ProviderBidConfiguration : IEntityTypeConfiguration<ProviderBid>
	{
		public void Configure(EntityTypeBuilder<ProviderBid> builder)
		{
			builder.Property(b => b.Status).HasConversion<string>().HasMaxLength(20);
			builder.Property(b => b.Notes).HasColumnType("nvarchar(max)");

			builder.OwnsOne(b => b.ProposedPrice, m =>
			{
				m.Property(x => x.Amount).HasColumnName("ProposedPrice").HasColumnType("decimal(18,2)");
				m.Property(x => x.Currency).HasColumnName("ProposedPriceCurrency").HasMaxLength(3);
			});

			builder.HasOne<AppUser>().WithMany().HasForeignKey(b => b.ProviderId).OnDelete(DeleteBehavior.Restrict);
			builder.HasIndex(b => new { b.CustomerJobId, b.ProviderId, b.Status });
		}
	}

}
