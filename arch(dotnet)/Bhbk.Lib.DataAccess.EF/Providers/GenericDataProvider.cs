using Bhbk.Lib.DataAccess.EF.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Bhbk.Lib.DataAccess.EF.Providers
{
    public class GenericDataProvider<TEntity> : BaseDataProvider<TEntity>
        where TEntity : class
    {
        public GenericDataProvider(IGenericUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public GenericDataProvider(DbContext context)
            : base(context)
        {
        }
    }
}
