using System.Data.Common;
using System.Data.Entity;

namespace Bhbk.Lib.DataAccess.EF.Tests.Models
{
    public partial class SampleEntities
    {
        public SampleEntities(DbConnection connection)
            : base(connection, true)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }
    }
}
