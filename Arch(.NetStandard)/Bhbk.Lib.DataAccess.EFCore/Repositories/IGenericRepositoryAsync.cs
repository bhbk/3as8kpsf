using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EFCore.Repositories
{
    public interface IGenericRepositoryAsync<TEntity>
        where TEntity : class
    {
        ValueTask<int> CountAsync(LambdaExpression lambda = null);
        ValueTask<TEntity> CreateAsync(TEntity entity);
        ValueTask<IEnumerable<TEntity>> CreateAsync(IEnumerable<TEntity> entities);
        ValueTask<TEntity> DeleteAsync(TEntity entity);
        ValueTask<IEnumerable<TEntity>> DeleteAsync(IEnumerable<TEntity> entities);
        ValueTask<IEnumerable<TEntity>> DeleteAsync(LambdaExpression lambda);
        ValueTask<bool> ExistsAsync(LambdaExpression lambda);
        ValueTask<IEnumerable<TEntity>> GetAsync(
            IEnumerable<Expression<Func<TEntity, object>>> expressions);
        ValueTask<IEnumerable<TEntity>> GetAsync(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null);
        ValueTask<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> predicates,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null);
        ValueTask<IEnumerable<TEntity>> GetAsNoTrackingAsync(
            IEnumerable<Expression<Func<TEntity, object>>> expressions);
        ValueTask<IEnumerable<TEntity>> GetAsNoTrackingAsync(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null);
        ValueTask<IEnumerable<TEntity>> GetAsNoTrackingAsync(
            Expression<Func<TEntity, bool>> predicates,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null);
        ValueTask<TEntity> UpdateAsync(TEntity entity);
        ValueTask<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities);
    }

    [Obsolete]
    public interface IGenericRepositoryAsync<TEntity, TKey> 
        where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity model);
        Task<bool> DeleteAsync(TKey key);
        Task<bool> ExistsAsync(TKey key);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicates = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null);
        Task<TEntity> UpdateAsync(TEntity model);
    }

    [Obsolete]
    public interface IGenericRepositoryAsync<TCreate, TEntity, TKey>
        where TCreate : class
        where TEntity : class
    {
        Task<TEntity> CreateAsync(TCreate model);
        Task<bool> DeleteAsync(TKey key);
        Task<bool> ExistsAsync(TKey key);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicates = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null);
        Task<TEntity> UpdateAsync(TEntity model);
    }
}
