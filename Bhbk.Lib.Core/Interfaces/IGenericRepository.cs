using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bhbk.Lib.Core.Interfaces
{
    public interface IGenericRepository<TEntity, TKey> 
        where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity key);
        Task<bool> ExistsAsync(TKey key);
        Task<TEntity> GetAsync(TKey key);
        Task<IQueryable<TEntity>> GetAsync(string sql, params object[] parameters);
        Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicates = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            bool tracking = true);
        Task<TEntity> UpdateAsync(TEntity entity);
    }

    public interface IGenericRepository<TCreate, TEntity, TUpdate, TKey>
        where TCreate : class
        where TEntity : class
        where TUpdate : class
    {
        Task<TEntity> CreateAsync(TCreate entity);
        Task<bool> DeleteAsync(TEntity key);
        Task<bool> ExistsAsync(TKey key);
        Task<TEntity> GetAsync(TKey key);
        Task<IQueryable<TEntity>> GetAsync(string sql, params object[] parameters);
        Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicates = null,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            bool tracking = true);
        Task<TEntity> UpdateAsync(TUpdate entity);
    }
}
