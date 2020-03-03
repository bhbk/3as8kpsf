using Bhbk.Lib.DataAccess.EF.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.Repositories
{
    public class GenericRepositoryAsync<TEntity> : IGenericRepositoryAsync<TEntity>, IDisposable
        where TEntity : class
    {
        protected readonly DbContext _context;

        public GenericRepositoryAsync(DbContext context)
        {
            _context = context ?? throw new NullReferenceException();
        }

        public virtual async Task<int> CountAsync(LambdaExpression lambda = null)
        {
            var result = _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .Count();

            return await Task.Run(() => result);
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

        public void Dispose()
        {
            _context.Dispose();
        }

        public virtual async Task<bool> ExistsAsync(LambdaExpression lambda)
        {
            var result = _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .Any();

            return await Task.Run(() => result);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            IEnumerable<Expression<Func<TEntity, object>>> expressions)
        {
            var results = _context.Set<TEntity>().AsQueryable()
                .Include(expressions)
                .ToList();

            return await Task.Run(() => results);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null)
        {
            var results = _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .Include(expressions)
                .ToList();

            return await Task.Run(() => results);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsNoTrackingAsync(
            IEnumerable<Expression<Func<TEntity, object>>> expressions)
        {
            var results = _context.Set<TEntity>()
                .Include(expressions)
                .AsNoTracking()
                .ToList();

            return await Task.Run(() => results);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsNoTrackingAsync(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null)
        {
            var results = _context.Set<TEntity>()
                .Compile(lambda)
                .Include(expressions)
                .AsNoTracking()
                .ToList();

            return await Task.Run(() => results);
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
