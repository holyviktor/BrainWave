using BrainWave.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BrainWave.Infrastructure.Data.EntityTypeConfiguration
{
    internal class ParticipantEntityConfiguration : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Participants)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasOne(x => x.Conversation)
                .WithMany(x => x.Participants)
                .HasForeignKey(x => x.ConversationId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

        }
    }
}
