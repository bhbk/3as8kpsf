using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bhbk.Lib.DataAccess.EF.Repositories
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        int Count(LambdaExpression lambda = null);
        TEntity Create(TEntity entity);
        IEnumerable<TEntity> Create(IEnumerable<TEntity> entity);
        TEntity Delete(TEntity entity);
        IEnumerable<TEntity> Delete(IEnumerable<TEntity> entities);
        IEnumerable<TEntity> Delete(LambdaExpression lambda);
        bool Exists(LambdaExpression lambda);
        IEnumerable<TEntity> Get(
            IEnumerable<Expression<Func<TEntity, object>>> expressions);
        IEnumerable<TEntity> Get(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null);
        IEnumerable<TEntity> GetAsNoTracking(
            IEnumerable<Expression<Func<TEntity, object>>> expressions);
        IEnumerable<TEntity> GetAsNoTracking(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null);
        TEntity Update(TEntity entity);
        IEnumerable<TEntity> Update(IEnumerable<TEntity> entities);
    }
}
