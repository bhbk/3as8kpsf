namespace Bhbk.Lib.DataAccess.EF.Tests.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            Roles = new HashSet<Roles>();
        }

        [Key]
        public Guid userID { get; set; }

        public Guid locationID { get; set; }

        [StringLength(128)]
        public string description { get; set; }

        public int int1 { get; set; }

        public int? int2 { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime date1 { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? date2 { get; set; }

        public decimal decimal1 { get; set; }

        public decimal? decimal2 { get; set; }

        public virtual Locations Locations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Roles> Roles { get; set; }
    }
}
