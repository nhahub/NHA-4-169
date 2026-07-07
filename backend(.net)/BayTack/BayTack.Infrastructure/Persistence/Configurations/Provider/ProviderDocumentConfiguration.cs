using BayTack.Domain.Entities.ProviderAggregate;
using BayTack.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace BayTack.Infrastructure.Persistence.Configurations.Provider
{

	public class ProviderDocumentConfiguration : IEntityTypeConfiguration<ProviderDocument>
	{
		public void Configure(EntityTypeBuilder<ProviderDocument> builder)
		{
			builder.Property(d => d.DocType).HasMaxLength(50).IsRequired();
			builder.Property(d => d.DocUrl).HasColumnType("nvarchar(max)").IsRequired();
			builder.Property(d => d.Status).HasConversion<string>().HasMaxLength(20);
			builder.HasOne<AppUser>().WithMany().HasForeignKey(d => d.ReviewedBy).OnDelete(DeleteBehavior.Restrict);
		}
	}

}
