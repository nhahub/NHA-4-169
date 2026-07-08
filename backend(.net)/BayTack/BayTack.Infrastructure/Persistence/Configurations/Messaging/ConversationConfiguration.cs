using BayTack.Domain.Entities.Messaging;
using BayTack.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.VisualBasic;

namespace BayTack.Infrastructure.Persistence.Configurations.Messaging
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.HasOne<AppUser>().WithMany().HasForeignKey(c => c.CustomerId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<AppUser>().WithMany().HasForeignKey(c => c.ProviderId).OnDelete(DeleteBehavior.Restrict);

            // One thread per customer/provider pair.
            builder.HasIndex(c => new { c.CustomerId, c.ProviderId }).IsUnique();
            builder.HasIndex(c => new { c.CustomerId, c.LastMessageAt });

            builder.HasMany(c => c.Messages).WithOne().HasForeignKey(m => m.ConversationId).OnDelete(DeleteBehavior.Cascade);
            builder.Metadata.FindNavigation(nameof(Conversation.Messages))!.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}