using System;
using System.Collections.Generic;

#nullable disable

namespace Bhbk.Lib.DataAccess.EFCore.Tests.Models
{
    public partial class User
    {
        public User()
        {
            Members = new HashSet<Member>();
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

        public virtual Location location { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
}
