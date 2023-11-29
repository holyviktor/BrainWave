using BrainWave.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainWave.Infrastructure.Data.EntityTypeConfiguration
{
    internal class ArticleComplaintEntityConfiguration:IEntityTypeConfiguration<ArticleComplaint>
    {
        public void Configure(EntityTypeBuilder<ArticleComplaint> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Status)
                 .WithMany(x => x.ArticleComplaints)
                 .HasForeignKey(x => x.StatusId)
                 .OnDelete(DeleteBehavior.NoAction)
                 .IsRequired();
            builder.HasOne(x => x.Article)
                 .WithMany(x => x.ArticleComplaints)
                 .HasForeignKey(x => x.ArticleId)
                 .OnDelete(DeleteBehavior.NoAction)
                 .IsRequired();
        }
    }
}
