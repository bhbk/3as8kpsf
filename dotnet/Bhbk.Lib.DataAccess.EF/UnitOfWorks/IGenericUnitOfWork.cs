using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace Bhbk.Lib.DataAccess.EF.UnitOfWorks
{
    public interface IGenericUnitOfWork : IDisposable
    {
        InstanceContext InstanceType { get; }
        DbContext DbContext { get; }
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
        void Commit(int? timeout = null);
    }
}
