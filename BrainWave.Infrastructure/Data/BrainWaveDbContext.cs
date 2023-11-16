using BrainWave.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainWave.Infrastructure.Data
{
    public class BrainWaveDbContext:DbContext
    {
        public BrainWaveDbContext(DbContextOptions options) : base(options) { }

        public BrainWaveDbContext() { }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Category> Categories { get;set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Saving> Savings { get; set; }
        public DbSet<User> Users { get;set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<StatusComplaint> StatusComplaints { get; set; }
        public DbSet<ReasonComplaint> ReasonComplaints { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BrainWaveDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }


    }
}
