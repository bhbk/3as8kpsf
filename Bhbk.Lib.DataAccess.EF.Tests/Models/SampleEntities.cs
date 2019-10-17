namespace Bhbk.Lib.DataAccess.EF.Tests.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SampleEntities : DbContext
    {
        public SampleEntities()
            : base("name=SampleEntities")
        {
        }

        public virtual DbSet<Locations> Locations { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Locations>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Locations)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Roles)
                .Map(m => m.ToTable("Members").MapLeftKey("roleID").MapRightKey("userID"));

            modelBuilder.Entity<Users>()
                .Property(e => e.decimal1)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Users>()
                .Property(e => e.decimal2)
                .HasPrecision(18, 0);
        }
    }
}
