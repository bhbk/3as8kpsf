using Bhbk.Lib.DataAccess.EF.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.UnitOfWorks
{
    public interface IGenericUnitOfWorkAsync : IGenericUnitOfWork, IAsyncDisposable
    {
        IGenericRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class;
        Task CommitAsync(CancellationToken ct = default);
    }
}
