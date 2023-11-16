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
    public class PaymentEntityConfiguration:IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.HasOne(x => x.Article)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.ArticleId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

        }
    }
}
