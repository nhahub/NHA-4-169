using BayTack.Domain.Entities.ProviderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BayTack.Infrastructure.Persistence.Configurations.Provider
{

	public class ProviderPortfolioItemConfiguration : IEntityTypeConfiguration<ProviderPortfolioItem>
	{
		public void Configure(EntityTypeBuilder<ProviderPortfolioItem> builder)
		{
			builder.Property(p => p.Title).HasMaxLength(150).IsRequired();
			builder.Property(p => p.Description).HasColumnType("nvarchar(max)");
			builder.Property(p => p.ImageUrl).HasColumnType("nvarchar(max)");
		}
	}
}
