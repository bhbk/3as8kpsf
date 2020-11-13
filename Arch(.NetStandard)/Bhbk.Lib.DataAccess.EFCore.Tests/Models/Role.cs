using System;
using System.Collections.Generic;

#nullable disable

namespace Bhbk.Lib.DataAccess.EFCore.Tests.Models
{
    public partial class Role
    {
        public Role()
        {
            Members = new HashSet<Member>();
        }

        public Guid roleID { get; set; }
        public string description { get; set; }

        public virtual ICollection<Member> Members { get; set; }
    }
}
