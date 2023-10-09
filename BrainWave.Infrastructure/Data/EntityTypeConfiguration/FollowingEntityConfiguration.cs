using BrainWave.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace BrainWave.Infrastructure.Data.EntityTypeConfiguration
{
    internal class FollowingEntityConfiguration : IEntityTypeConfiguration<Following>
    {
        public void Configure(EntityTypeBuilder<Following> builder)
        {
            builder.HasKey(x => x.Id);


            builder.HasOne(x => x.User)
                 .WithMany(x => x.Followings)
                 .HasForeignKey(x => x.FollowingUserId)
                 .OnDelete(DeleteBehavior.NoAction)
                 .IsRequired();

            builder.HasOne(x => x.User)
                 .WithMany(x => x.Followings)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.NoAction)
                 .IsRequired();
        }
    }
}
