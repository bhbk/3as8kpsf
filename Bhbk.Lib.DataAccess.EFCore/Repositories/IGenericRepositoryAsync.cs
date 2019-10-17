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
        Task<int> CountAsync(LambdaExpression lambda = null);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> DeleteAsync(TEntity entity);
        Task<IEnumerable<TEntity>> DeleteAsync(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> DeleteAsync(LambdaExpression lambda);
        Task<bool> ExistsAsync(LambdaExpression lambda);
        Task<IEnumerable<TEntity>> GetAsync(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null);
        Task<IEnumerable<TEntity>> GetAsync(
            IEnumerable<Expression<Func<TEntity, object>>> expressions);
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> predicates,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null);
        Task<TEntity> UpdateAsync(TEntity entity);
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
