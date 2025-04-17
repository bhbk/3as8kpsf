using Bhbk.Lib.DataState.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bhbk.Lib.DataAccess.EF.Providers
{
    public interface IBaseDataProvider<TEntity> : IDisposable where TEntity : class
    {
        IQueryable<TEntity> Query(bool asNoTracking = true);

        TEntity Find(params object[] keyValues);

        IEnumerable<TEntity> Select(
            Expression<Func<TEntity, bool>> predicate = null,
            bool asNoTracking = true,
            IEnumerable<Expression<Func<TEntity, object>>> includes = null);

        IDataStateResult<TEntity> Select(
            IDataState state,
            bool asNoTracking = true,
            IEnumerable<Expression<Func<TEntity, object>>> includes = null);

        IEnumerable<TResult> Select<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            bool asNoTracking = true);

        TEntity Post(TEntity entity, bool commit = true);

        TEntity Put(TEntity entity, bool commit = true);

        void Delete(TEntity entity, bool commit = true);

        void Delete(Expression<Func<TEntity, bool>> predicate, bool commit = true);
    }
}
