using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.Repositories
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
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
