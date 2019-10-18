using System;
using System.Collections.Generic;

namespace Bhbk.Lib.DataAccess.EFCore.Tests.Models
{
    public partial class Members
    {
        public Guid userID { get; set; }
        public Guid roleID { get; set; }

        public virtual Roles role { get; set; }
        public virtual Users user { get; set; }
    }
}
