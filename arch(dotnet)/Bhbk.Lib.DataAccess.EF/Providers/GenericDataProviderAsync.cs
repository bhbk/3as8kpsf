using Bhbk.Lib.DataAccess.EF.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace Bhbk.Lib.DataAccess.EF.Providers
{
    public class GenericDataProviderAsync<TEntity> : BaseDataProviderAsync<TEntity>
        where TEntity : class
    {
        public GenericDataProviderAsync(IGenericUnitOfWorkAsync unitOfWork)
            : base(unitOfWork)
        {
        }

        public GenericDataProviderAsync(DbContext context)
            : base(context)
        {
        }
    }
}
