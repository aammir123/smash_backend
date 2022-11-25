using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Smash_Backend.Entities
{
    public partial class SmashDBContext : DbContext
    {
        public SmashDBContext()
        {
        }

        public SmashDBContext(DbContextOptions<SmashDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EntryExitTransactions> EntryExitTransactions { get; set; }
        public virtual DbSet<Interchanges> Interchanges { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=appConfiguration:ConnectionString");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EntryExitTransactions>(entity =>
            {
                entity.Property(e => e.NumberPlate).IsFixedLength();
            });

            modelBuilder.Entity<Interchanges>(entity =>
            {
                entity.Property(e => e.Name).IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
