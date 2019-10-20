using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EF.Extensions;
using Bhbk.Lib.DataAccess.EF.UnitOfWorks;
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
            var result = _context.Set<TEntity>()
                .Add(entity);

            return await Task.Run(() => result);
        }

        public virtual async Task<IEnumerable<TEntity>> CreateAsync(IEnumerable<TEntity> entities)
        {
            var results = new List<TEntity>();

            foreach (var entity in entities)
            {
                var result = _context.Set<TEntity>()
                    .Add(entity);

                results.Add(result);
            }

            return await Task.Run(() => results);
        }

        public virtual async Task<TEntity> DeleteAsync(TEntity entity)
        {
            var result = _context.Set<TEntity>()
                .Remove(entity);

            return await Task.Run(() => result);
        }

        public virtual async Task<IEnumerable<TEntity>> DeleteAsync(IEnumerable<TEntity> entities)
        {
            var results = new List<TEntity>();

            foreach (var entity in entities)
            {
                var result = _context.Set<TEntity>()
                    .Remove(entity);

                results.Add(result);
            }

            return await Task.Run(() => results);
        }

        public virtual async Task<IEnumerable<TEntity>> DeleteAsync(LambdaExpression lambda)
        {
            var entities = _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .ToList();

            return await DeleteAsync(entities);
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

        public virtual async Task<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities)
        {
            var results = new List<TEntity>();

            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Modified;

                var result = _context.Entry(entity).Entity;

                results.Add(result);
            }

            return await Task.Run(() => results);
        }
    }
}
