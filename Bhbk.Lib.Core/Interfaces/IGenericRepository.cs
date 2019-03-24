using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bhbk.Lib.Core.Interfaces
{
    public interface IGenericRepository<TModel, TKey> 
        where TModel : class
    {
        Task<TModel> CreateAsync(TModel entity);
        Task<bool> DeleteAsync(TModel key);
        Task<bool> ExistsAsync(TKey key);
        Task<IQueryable<TModel>> GetAsync(Expression<Func<TModel, bool>> predicates = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> includes = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orders = null,
            int? skip = null,
            int? take = null);
        Task<TModel> UpdateAsync(TModel entity);
    }

    public interface IGenericRepository<TCreate, TModel, TKey>
        where TCreate : class
        where TModel : class
    {
        Task<TModel> CreateAsync(TCreate entity);
        Task<bool> DeleteAsync(TKey key);
        Task<bool> ExistsAsync(TKey key);
        Task<IEnumerable<TModel>> GetAsync(Expression<Func<TModel, bool>> predicates = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> includes = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orders = null,
            int? skip = null,
            int? take = null);
        Task<TModel> UpdateAsync(TModel entity);
    }
}
