﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.Repositories
{
    public interface IGenericRepositoryAsync<TEntity>
        where TEntity : class
    {
        Task<int> CountAsync(LambdaExpression lambda = null);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> CreateAsync(IEnumerable<TEntity> entities);
        Task<TEntity> DeleteAsync(TEntity entity);
        Task<IEnumerable<TEntity>> DeleteAsync(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> DeleteAsync(LambdaExpression lambda);
        Task<bool> ExistsAsync(LambdaExpression lambda);
        Task<IEnumerable<TEntity>> GetAsync(
            IEnumerable<Expression<Func<TEntity, object>>> expressions);
        Task<IEnumerable<TEntity>> GetAsync(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null);
        Task<IEnumerable<TEntity>> GetAsNoTrackingAsync(
            IEnumerable<Expression<Func<TEntity, object>>> expressions);
        Task<IEnumerable<TEntity>> GetAsNoTrackingAsync(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities);
    }
}
