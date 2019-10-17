using System;
using System.Collections.Generic;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.Models
{
    public partial class Locations
    {
        public Locations()
        {
            Users = new HashSet<Users>();
        }

        public Guid locationID { get; set; }
        public string description { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
