using Bhbk.Lib.DataAccess.EF.Extensions;
using Bhbk.Lib.DataState.Extensions;
using Bhbk.Lib.DataState.Interfaces;
using Bhbk.Lib.DataState.Models;
using Bhbk.Lib.QueryExpression.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.Repositories
{
    public class GenericRepositoryAsync<TEntity> : IGenericRepositoryAsync<TEntity>, IAsyncDisposable, IDisposable
        where TEntity : class
    {
        protected readonly DbContext _context;

        public GenericRepositoryAsync(DbContext context)
        {
            _context = context ?? throw new NullReferenceException();
        }

        public virtual async ValueTask<int> CountAsync(LambdaExpression lambda = null, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            return await _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .CountAsync(ct);
        }

        public virtual async ValueTask<TEntity> PostAsync(TEntity entity, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var result = await _context.Set<TEntity>()
                .AddAsync(entity, ct);

            return result.Entity;
        }

        public virtual async ValueTask<IEnumerable<TEntity>> PostAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var results = new List<TEntity>();

            foreach (var entity in entities)
            {
                ct.ThrowIfCancellationRequested();

                var result = await _context.Set<TEntity>()
                    .AddAsync(entity, ct);

                results.Add(result.Entity);
            }

            return results;
        }

        public virtual ValueTask<TEntity> DeleteAsync(TEntity entity, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var result = _context.Set<TEntity>()
                .Remove(entity).Entity;

            return new ValueTask<TEntity>(result);
        }

        public virtual ValueTask<IEnumerable<TEntity>> DeleteAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var results = new List<TEntity>();

            foreach (var entity in entities)
            {
                ct.ThrowIfCancellationRequested();

                var result = _context.Set<TEntity>()
                    .Remove(entity).Entity;

                results.Add(result);
            }

            return new ValueTask<IEnumerable<TEntity>>(results);
        }

        public virtual async ValueTask<IEnumerable<TEntity>> DeleteAsync(LambdaExpression lambda, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var entities = await _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .ToListAsync(ct);

            return await DeleteAsync(entities, ct);
        }

        public virtual async ValueTask<int> DeleteBulkAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return await _context.Set<TEntity>()
                .Where(predicate)
                .ExecuteDeleteAsync(ct);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return _context.DisposeAsync();
        }

        public virtual async ValueTask<bool> ExistsAsync(LambdaExpression lambda, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            return await _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .AnyAsync(ct);
        }

        public virtual async ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken ct = default)
        {
            return await _context.Set<TEntity>().FindAsync(keyValues, ct);
        }

        public virtual async ValueTask<IEnumerable<TEntity>> GetAsync(
            IEnumerable<Expression<Func<TEntity, object>>> expressions,
            CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            return await _context.Set<TEntity>().AsQueryable()
                .Include(expressions)
                .ToListAsync(ct);
        }

        public virtual async ValueTask<IEnumerable<TEntity>> GetAsync(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null,
            CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            return await _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .Include(expressions)
                .ToListAsync(ct);
        }

        public virtual async ValueTask<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicates,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null,
            CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

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

            return await query.ToListAsync(ct);
        }

        public virtual async ValueTask<IEnumerable<TEntity>> GetAsNoTrackingAsync(
            IEnumerable<Expression<Func<TEntity, object>>> expressions,
            CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            return await _context.Set<TEntity>()
                .Include(expressions)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public virtual async ValueTask<IEnumerable<TEntity>> GetAsNoTrackingAsync(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null,
            CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            return await _context.Set<TEntity>()
                .Compile(lambda)
                .Include(expressions)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public virtual async ValueTask<IEnumerable<TEntity>> GetAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicates,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orders = null,
            int? skip = null,
            int? take = null,
            CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

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

            return await query.AsNoTracking().ToListAsync(ct);
        }

        public virtual async ValueTask<TEntity> PutAsync(TEntity entity, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var result = _context.Set<TEntity>()
                .Update(entity).Entity;

            return await Task.FromResult(result);
        }

        public virtual async ValueTask<IEnumerable<TEntity>> PutAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var results = new List<TEntity>();

            foreach (var entity in entities)
            {
                ct.ThrowIfCancellationRequested();

                var result = _context.Set<TEntity>()
                    .Update(entity).Entity;

                results.Add(result);
            }

            return await Task.FromResult<IEnumerable<TEntity>>(results);
        }

        public virtual async ValueTask<int> UpdateBulkAsync(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
            CancellationToken ct = default)
        {
            return await _context.Set<TEntity>()
                .Where(predicate)
                .ExecuteUpdateAsync(setPropertyCalls, ct);
        }

        public virtual IQueryable<TEntity> Query(bool asNoTracking = true)
        {
            return asNoTracking
                ? _context.Set<TEntity>().AsNoTracking()
                : _context.Set<TEntity>().AsQueryable();
        }

        public virtual async ValueTask<IDataStateResult<TEntity>> GetAsync(
            IDataState state,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            bool asNoTracking = true,
            CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var query = asNoTracking
                ? _context.Set<TEntity>().AsNoTracking()
                : _context.Set<TEntity>().AsQueryable();

            if (includes != null)
                query = includes(query);

            // Get total count before paging (apply only filter predicate)
            var countLambda = QueryExpressionFactory.GetQueryExpression<TEntity>()
                .ApplyPredicate(state);
            var total = countLambda != null
                ? await query.Compile(countLambda).CountAsync(ct)
                : await query.CountAsync(ct);

            // Apply full state (filter + sort + paging)
            var stateLambda = QueryExpressionFactory.GetQueryExpression<TEntity>()
                .ApplyState(state);
            var data = await query.Compile(stateLambda).ToListAsync(ct);

            return new PagerStateResult<TEntity> { Data = data, Total = total };
        }

        public virtual async ValueTask<IEnumerable<TResult>> SelectAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            bool asNoTracking = true,
            CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var query = asNoTracking
                ? _context.Set<TEntity>().AsNoTracking()
                : _context.Set<TEntity>().AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            return await query.Select(selector).ToListAsync(ct);
        }
    }
}
