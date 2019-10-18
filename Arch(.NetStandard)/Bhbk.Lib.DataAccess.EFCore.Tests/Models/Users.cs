using System;
using System.Collections.Generic;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.Models
{
    public partial class Users
    {
        public Users()
        {
            Members = new HashSet<Members>();
        }

        public Guid userID { get; set; }
        public Guid locationID { get; set; }
        public string description { get; set; }
        public int int1 { get; set; }
        public int? int2 { get; set; }
        public DateTime date1 { get; set; }
        public DateTime? date2 { get; set; }
        public decimal decimal1 { get; set; }
        public decimal? decimal2 { get; set; }

        public virtual Locations location { get; set; }
        public virtual ICollection<Members> Members { get; set; }
    }
}
