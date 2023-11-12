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
    internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.Surname)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.Tag)
                .IsUnique();

            builder.Property(x => x.Tag)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

        }
    }
}