using Bhbk.Lib.DataState.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bhbk.Lib.DataAccess.EF.Repositories
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        int Count(LambdaExpression lambda = null);
        TEntity Post(TEntity entity);
        IEnumerable<TEntity> Post(IEnumerable<TEntity> entities);
        TEntity Delete(TEntity entity);
        IEnumerable<TEntity> Delete(IEnumerable<TEntity> entities);
        IEnumerable<TEntity> Delete(LambdaExpression lambda);
        int DeleteBulk(Expression<Func<TEntity, bool>> predicate);
        bool Exists(LambdaExpression lambda);
        TEntity Find(params object[] keyValues);
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
        IEnumerable<TEntity> GetAsNoTracking(
            IEnumerable<Expression<Func<TEntity, object>>> expressions);
        IEnumerable<TEntity> GetAsNoTracking(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null);
        IEnumerable<TEntity> GetAsNoTracking(
            Expression<Func<TEntity, bool>> predicates,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null);
        IDataStateResult<TEntity> Get(
            IDataState state,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            bool asNoTracking = true);
        IEnumerable<TResult> Select<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            bool asNoTracking = true);
        IQueryable<TEntity> Query(bool asNoTracking = true);
        TEntity Put(TEntity entity);
        IEnumerable<TEntity> Put(IEnumerable<TEntity> entities);
        int UpdateBulk(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<TEntity>, Microsoft.EntityFrameworkCore.Query.SetPropertyCalls<TEntity>>> setPropertyCalls);
    }

    [Obsolete]
    public interface IGenericRepository<TEntity, TKey>
        where TEntity : class
    {
        TEntity Post(TEntity model);
        bool Delete(TKey key);
        bool Exists(TKey key);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicates = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null);
        TEntity Put(TEntity model);
    }

    [Obsolete]
    public interface IGenericRepository<TCreate, TEntity, TKey>
        where TCreate : class
        where TEntity : class
    {
        TEntity Post(TCreate model);
        bool Delete(TKey key);
        bool Exists(TKey key);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicates = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null);
        TEntity Put(TEntity model);
    }
}
