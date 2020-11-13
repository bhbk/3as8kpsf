using System;
using System.Collections.Generic;

#nullable disable

namespace Bhbk.Lib.DataAccess.EFCore.Tests.Models
{
    public partial class Location
    {
        public Location()
        {
            Users = new HashSet<User>();
        }

        public Guid locationID { get; set; }
        public string description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
