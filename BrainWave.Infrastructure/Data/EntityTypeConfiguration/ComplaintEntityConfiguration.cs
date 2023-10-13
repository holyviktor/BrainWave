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
    public class ComplaintEntityConfiguration : IEntityTypeConfiguration<Complaint>
    {
        public void Configure(EntityTypeBuilder<Complaint> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Text)
                .HasMaxLength(300)
                .IsRequired();

            builder.HasOne(x => x.User)
                 .WithMany(x => x.Complaints)
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.NoAction)
                 .IsRequired();

            builder.HasOne(x => x.Article)
                 .WithMany(x => x.Complaints)
                 .HasForeignKey(x => x.ArticleId)
                 .OnDelete(DeleteBehavior.NoAction)
                 .IsRequired();

            builder.HasOne(x => x.Reason)
                 .WithMany(x => x.Complaints)
                 .HasForeignKey(x => x.ReasonId)
                 .OnDelete(DeleteBehavior.NoAction)
                 .IsRequired();

            builder.HasOne(x => x.Status)
                 .WithMany(x => x.Complaints)
                 .HasForeignKey(x => x.StatusId)
                 .OnDelete(DeleteBehavior.NoAction)
                 .IsRequired();

        }
    }
}
