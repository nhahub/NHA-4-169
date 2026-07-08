using BayTack.Domain.Entities.Messaging;
using BayTack.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BayTack.Infrastructure.Persistence.Configurations.Messaging
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(m => m.Text).HasColumnType("nvarchar(max)").IsRequired();
            builder.HasOne<AppUser>().WithMany().HasForeignKey(m => m.SenderId).OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(m => new { m.ConversationId, m.SentAt });
        }
    }
}