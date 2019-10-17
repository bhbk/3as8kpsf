using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.Models
{
    public partial class SampleEntities : DbContext
    {
        public SampleEntities()
        {
        }

        public SampleEntities(DbContextOptions<SampleEntities> options)
            : base(options)
        {
        }

        public virtual DbSet<Locations> Locations { get; set; }
        public virtual DbSet<Members> Members { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Locations>(entity =>
            {
                entity.HasKey(e => e.locationID);

                entity.HasIndex(e => e.locationID)
                    .HasName("IX_Locations")
                    .IsUnique();

                entity.Property(e => e.locationID).ValueGeneratedNever();
            });

            modelBuilder.Entity<Members>(entity =>
            {
                entity.HasKey(e => new { e.userID, e.roleID });

                entity.HasIndex(e => new { e.userID, e.roleID })
                    .HasName("IX_Members")
                    .IsUnique();

                entity.HasOne(d => d.role)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.roleID)
                    .HasConstraintName("FK_Members_RoleID");

                entity.HasOne(d => d.user)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.userID)
                    .HasConstraintName("FK_Members_UserID");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.roleID);

                entity.HasIndex(e => e.roleID)
                    .HasName("IX_Roles")
                    .IsUnique();

                entity.Property(e => e.roleID).ValueGeneratedNever();
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.userID);

                entity.HasIndex(e => e.userID)
                    .HasName("IX_Users")
                    .IsUnique();

                entity.Property(e => e.userID).ValueGeneratedNever();

                entity.Property(e => e.decimal1).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.decimal2).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.description).HasMaxLength(128);

                entity.HasOne(d => d.location)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.locationID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_LocationID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
