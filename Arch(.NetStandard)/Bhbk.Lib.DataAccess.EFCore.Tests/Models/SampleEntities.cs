using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

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

        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasIndex(e => e.locationID, "IX_Locations")
                    .IsUnique();

                entity.Property(e => e.locationID).ValueGeneratedNever();
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => new { e.userID, e.roleID });

                entity.HasIndex(e => new { e.userID, e.roleID }, "IX_Members")
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

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.roleID, "IX_Roles")
                    .IsUnique();

                entity.Property(e => e.roleID).ValueGeneratedNever();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.userID, "IX_Users")
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
