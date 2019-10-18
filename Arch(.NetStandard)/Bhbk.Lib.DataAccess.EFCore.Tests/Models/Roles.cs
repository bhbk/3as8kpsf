using System;
using System.Collections.Generic;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.Models
{
    public partial class Roles
    {
        public Roles()
        {
            Members = new HashSet<Members>();
        }

        public Guid roleID { get; set; }
        public string description { get; set; }

        public virtual ICollection<Members> Members { get; set; }
    }
}
