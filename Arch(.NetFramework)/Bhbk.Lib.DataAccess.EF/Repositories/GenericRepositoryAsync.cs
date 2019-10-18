using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EF.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.Repositories
{
    public class GenericRepositoryAsync<TEntity> : IGenericRepositoryAsync<TEntity>
        where TEntity : class
    {
        protected readonly InstanceContext _instance;
        protected readonly DbContext _context;

        public GenericRepositoryAsync(DbContext context)
        {
            _context = context ?? throw new NullReferenceException();
            _instance = InstanceContext.DeployedOrLocal;
        }

        public GenericRepositoryAsync(DbContext context, InstanceContext instance)
        {
            _context = context ?? throw new NullReferenceException();
            _instance = instance;
        }

        public virtual async Task<int> CountAsync(LambdaExpression lambda = null)
        {
            return await _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .CountAsync();
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            return await Task.Run(() => _context.Set<TEntity>()
                .Add(entity));
        }

        public virtual async Task<TEntity> DeleteAsync(TEntity entity)
        {
            return await Task.Run(() => _context.Set<TEntity>()
                .Remove(entity));
        }

        public virtual async Task<IEnumerable<TEntity>> DeleteAsync(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => _context.Set<TEntity>()
                .RemoveRange(entities));

            return entities;
        }

        public virtual async Task<IEnumerable<TEntity>> DeleteAsync(LambdaExpression lambda)
        {
            var entities = await _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .ToListAsync();

            _context.Set<TEntity>()
                .RemoveRange(entities);

            return entities;
        }

        public virtual async Task<bool> ExistsAsync(LambdaExpression lambda)
        {
            return await _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .AnyAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            IEnumerable<Expression<Func<TEntity, object>>> expressions)
        {
            return await _context.Set<TEntity>().AsQueryable()
                .Include(expressions)
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null)
        {
            return await _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .Include(expressions)
                .ToListAsync();
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            return await Task.Run(() =>
                _context.Entry(entity).Entity);
        }
    }
}
