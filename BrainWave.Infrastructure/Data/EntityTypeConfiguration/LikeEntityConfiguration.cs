using BrainWave.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainWave.Infrastructure.Data.EntityTypeConfiguration
{
    internal class LikeEntityConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x=>x.User)
                .WithMany(x=>x.Likes)
                .HasForeignKey(x=>x.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasOne(x=>x.Article)
                .WithMany(x => x.Likes)
                .HasForeignKey(x=>x.ArticleId) 
                .OnDelete(DeleteBehavior.NoAction) 
                .IsRequired();

        }
    }
}
