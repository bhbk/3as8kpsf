using System;
using System.Collections.Generic;

#nullable disable

namespace Bhbk.Lib.DataAccess.EFCore.Tests.Models
{
    public partial class Member
    {
        public Guid userID { get; set; }
        public Guid roleID { get; set; }

        public virtual Role role { get; set; }
        public virtual User user { get; set; }
    }
}
