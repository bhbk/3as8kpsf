using Bhbk.Lib.DataState.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.Providers
{
    public interface IBaseDataProviderAsync<TEntity> : IAsyncDisposable where TEntity : class
    {
        IQueryable<TEntity> Query(bool asNoTracking = true);

        ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken ct = default);

        ValueTask<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            bool asNoTracking = true,
            IEnumerable<Expression<Func<TEntity, object>>> includes = null,
            CancellationToken ct = default);

        ValueTask<IDataStateResult<TEntity>> SelectAsync(
            IDataState state,
            bool asNoTracking = true,
            IEnumerable<Expression<Func<TEntity, object>>> includes = null,
            CancellationToken ct = default);

        ValueTask<IEnumerable<TResult>> SelectAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            bool asNoTracking = true,
            CancellationToken ct = default);

        ValueTask<TEntity> PostAsync(TEntity entity, bool commit = true, CancellationToken ct = default);

        ValueTask<TEntity> PutAsync(TEntity entity, bool commit = true, CancellationToken ct = default);

        ValueTask DeleteAsync(TEntity entity, bool commit = true, CancellationToken ct = default);

        ValueTask DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool commit = true, CancellationToken ct = default);
    }

    [Obsolete("Use IBaseDataProvider<TEntity> or IBaseDataProviderAsync<TEntity> instead.")]
    public interface IDataProviderAsync<TEntity> : IBaseDataProvider<TEntity>, IBaseDataProviderAsync<TEntity>
        where TEntity : class
    {
    }
}
