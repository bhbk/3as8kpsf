using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bhbk.Lib.DataAccess.EFCore.Repositories
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        int Count(LambdaExpression lambda = null);
        TEntity Create(TEntity entity);
        TEntity Delete(TEntity entity);
        IEnumerable<TEntity> Delete(IEnumerable<TEntity> entities);
        IEnumerable<TEntity> Delete(LambdaExpression lambda);
        bool Exists(LambdaExpression lambda);
        IEnumerable<TEntity> Get(
            IEnumerable<Expression<Func<TEntity, object>>> expressions);
        IEnumerable<TEntity> Get(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null);
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> predicates,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null);
        TEntity Update(TEntity entity);
    }

    [Obsolete]
    public interface IGenericRepository<TEntity, TKey>
        where TEntity : class
    {
        TEntity Create(TEntity model);
        bool Delete(TKey key);
        bool Exists(TKey key);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicates = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null);
        TEntity Update(TEntity model);
    }

    [Obsolete]
    public interface IGenericRepository<TCreate, TEntity, TKey>
        where TCreate : class
        where TEntity : class
    {
        TEntity Create(TCreate model);
        bool Delete(TKey key);
        bool Exists(TKey key);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicates = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null);
        TEntity Update(TEntity model);
    }
}
