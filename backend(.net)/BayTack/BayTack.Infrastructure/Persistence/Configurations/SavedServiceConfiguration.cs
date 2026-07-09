using BayTack.Domain.Entities.ServiceAggregate;
using BayTack.Domain.Entities.UserFeatures;
using BayTack.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceEntity = BayTack.Domain.Entities.ServiceAggregate.Service;

namespace BayTack.Infrastructure.Persistence.Configurations
{
    public class SavedServiceConfiguration : IEntityTypeConfiguration<SavedService>
    {
        public void Configure(EntityTypeBuilder<SavedService> builder)
        {
            builder.HasOne<AppUser>().WithMany().HasForeignKey(s => s.CustomerId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<ServiceEntity>()
            .WithMany()
             .HasForeignKey(s => s.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(s => new { s.CustomerId, s.ServiceId }).IsUnique();
        }
    }
}