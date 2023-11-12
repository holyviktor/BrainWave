using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BrainWave.Core.Entities;

namespace BrainWave.Infrastructure.Data.EntityTypeConfiguration
{
    internal class MessageEntityConfiguration:IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasOne(x => x.Conversation)
                .WithMany(x => x.Messages)
                .HasForeignKey(x => x.ConversationId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

        }
    }
}
