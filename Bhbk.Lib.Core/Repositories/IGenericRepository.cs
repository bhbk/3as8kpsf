using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bhbk.Lib.Core.Repositories
{
    public interface IGenericRepository<TModel, TKey>
        where TModel : class
    {
        TModel Create(TModel model);
        bool Delete(TKey key);
        bool Exists(TKey key);
        IEnumerable<TModel> Get(Expression<Func<TModel, bool>> predicates = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> includes = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orders = null,
            int? skip = null,
            int? take = null);
        TModel Update(TModel model);
    }

    public interface IGenericRepository<TCreate, TModel, TKey>
        where TCreate : class
        where TModel : class
    {
        TModel Create(TCreate model);
        bool Delete(TKey key);
        bool Exists(TKey key);
        IEnumerable<TModel> Get(Expression<Func<TModel, bool>> predicates = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> includes = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orders = null,
            int? skip = null,
            int? take = null);
        TModel Update(TModel model);
    }
}
