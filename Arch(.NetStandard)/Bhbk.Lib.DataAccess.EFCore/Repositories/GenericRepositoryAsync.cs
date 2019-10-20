using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EFCore.Repositories
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

        public virtual async ValueTask CommitAsync()
        {
            var entities = from deltas in _context.ChangeTracker.Entries()
                           where deltas.State == EntityState.Added || deltas.State == EntityState.Modified
                           select deltas.Entity;

            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);

                Validator.ValidateObject(
                    entity,
                    validationContext,
                    validateAllProperties: true);
            }

            await _context.SaveChangesAsync();
        }

        public virtual async ValueTask<int> CountAsync(LambdaExpression lambda = null)
        {
            return await _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .CountAsync();
        }

        public virtual async ValueTask<TEntity> CreateAsync(TEntity entity)
        {
            var result = (await _context.Set<TEntity>()
                .AddAsync(entity)).Entity;

            return result;
        }

        public virtual async ValueTask<IEnumerable<TEntity>> CreateAsync(IEnumerable<TEntity> entities)
        {
            var results = new List<TEntity>();

            foreach (var entity in entities)
            {
                var result = (await _context.Set<TEntity>()
                    .AddAsync(entity)).Entity;

                results.Add(result);
            }

            return results;
        }

        public virtual async ValueTask<TEntity> DeleteAsync(TEntity entity)
        {
            var result = _context.Set<TEntity>()
                .Remove(entity).Entity;

            return await Task.Run(() => result);
        }

        public virtual async ValueTask<IEnumerable<TEntity>> DeleteAsync(IEnumerable<TEntity> entities)
        {
            var results = new List<TEntity>();

            foreach (var entity in entities)
            {
                var result = _context.Set<TEntity>()
                    .Remove(entity).Entity;

                results.Add(result);
            }

            return await Task.Run(() => results);
        }

        public virtual async ValueTask<IEnumerable<TEntity>> DeleteAsync(LambdaExpression lambda)
        {
            var entities = _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .ToList();

            return await DeleteAsync(entities);
        }

        public virtual async ValueTask<bool> ExistsAsync(LambdaExpression lambda)
        {
            return await _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .AnyAsync();
        }

        public virtual async ValueTask<IEnumerable<TEntity>> GetAsync(
            IEnumerable<Expression<Func<TEntity, object>>> expressions)
        {
            return await _context.Set<TEntity>().AsQueryable()
                .Include(expressions)
                .ToListAsync();
        }

        public virtual async ValueTask<IEnumerable<TEntity>> GetAsync(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null)
        {
            return await _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .Include(expressions)
                .ToListAsync();
        }

        [Obsolete]
        public virtual async ValueTask<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicates,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (predicates != null)
                query = query.Where(predicates);

            if (includes != null)
                query = includes(query);

            if (orders != null)
            {
                query = orders(query)
                    .Skip(skip.Value)
                    .Take(take.Value);
            }

            return await query.ToListAsync();
        }

        public virtual async ValueTask<TEntity> UpdateAsync(TEntity entity)
        {
            var result = _context.Set<TEntity>()
                .Update(entity).Entity;

            return await Task.Run(() => result);
        }

        public virtual async ValueTask<IEnumerable<TEntity>> UpdateAsync(IEnumerable<TEntity> entities)
        {
            var results = new List<TEntity>();

            foreach (var entity in entities)
            {
                var result = _context.Set<TEntity>()
                    .Update(entity).Entity;

                results.Add(result);
            }

            return await Task.Run(() => results);
        }
    }
}
