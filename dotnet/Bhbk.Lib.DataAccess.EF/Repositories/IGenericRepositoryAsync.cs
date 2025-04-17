using Bhbk.Lib.DataState.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.Repositories
{
    public interface IGenericRepositoryAsync<TEntity>
        where TEntity : class
    {
        ValueTask<int> CountAsync(LambdaExpression lambda = null, CancellationToken ct = default);
        ValueTask<TEntity> PostAsync(TEntity entity, CancellationToken ct = default);
        ValueTask<IEnumerable<TEntity>> PostAsync(IEnumerable<TEntity> entities, CancellationToken ct = default);
        ValueTask<TEntity> DeleteAsync(TEntity entity, CancellationToken ct = default);
        ValueTask<IEnumerable<TEntity>> DeleteAsync(IEnumerable<TEntity> entities, CancellationToken ct = default);
        ValueTask<IEnumerable<TEntity>> DeleteAsync(LambdaExpression lambda, CancellationToken ct = default);
        ValueTask<int> DeleteBulkAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
        ValueTask<bool> ExistsAsync(LambdaExpression lambda, CancellationToken ct = default);
        ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken ct = default);
        ValueTask<IEnumerable<TEntity>> GetAsync(
            IEnumerable<Expression<Func<TEntity, object>>> expressions,
            CancellationToken ct = default);
        ValueTask<IEnumerable<TEntity>> GetAsync(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null,
            CancellationToken ct = default);
        ValueTask<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> predicates,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null,
            CancellationToken ct = default);
        ValueTask<IEnumerable<TEntity>> GetAsNoTrackingAsync(
            IEnumerable<Expression<Func<TEntity, object>>> expressions,
            CancellationToken ct = default);
        ValueTask<IEnumerable<TEntity>> GetAsNoTrackingAsync(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null,
            CancellationToken ct = default);
        ValueTask<IEnumerable<TEntity>> GetAsNoTrackingAsync(
            Expression<Func<TEntity, bool>> predicates,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null,
            CancellationToken ct = default);
        ValueTask<IDataStateResult<TEntity>> GetAsync(
            IDataState state,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            bool asNoTracking = true,
            CancellationToken ct = default);
        ValueTask<IEnumerable<TResult>> SelectAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            bool asNoTracking = true,
            CancellationToken ct = default);
        IQueryable<TEntity> Query(bool asNoTracking = true);
        ValueTask<TEntity> PutAsync(TEntity entity, CancellationToken ct = default);
        ValueTask<IEnumerable<TEntity>> PutAsync(IEnumerable<TEntity> entities, CancellationToken ct = default);
        ValueTask<int> UpdateBulkAsync(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<TEntity>, Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<TEntity>>> setPropertyCalls,
            CancellationToken ct = default);
    }

    [Obsolete]
    public interface IGenericRepositoryAsync<TEntity, TKey>
        where TEntity : class
    {
        Task<TEntity> PostAsync(TEntity model);
        Task<bool> DeleteAsync(TKey key);
        Task<bool> ExistsAsync(TKey key);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicates = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null);
        Task<TEntity> PutAsync(TEntity model);
    }

    [Obsolete]
    public interface IGenericRepositoryAsync<TCreate, TEntity, TKey>
        where TCreate : class
        where TEntity : class
    {
        Task<TEntity> PostAsync(TCreate model);
        Task<bool> DeleteAsync(TKey key);
        Task<bool> ExistsAsync(TKey key);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicates = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null);
        Task<TEntity> PutAsync(TEntity model);
    }
}
