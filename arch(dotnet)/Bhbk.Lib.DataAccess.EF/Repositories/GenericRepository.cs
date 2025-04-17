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

namespace Bhbk.Lib.DataAccess.EF.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>, IDisposable
        where TEntity : class
    {
        protected readonly DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context ?? throw new NullReferenceException();
        }

        public virtual int Count(LambdaExpression lambda = null)
        {
            return _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .Count();
        }

        public virtual TEntity Post(TEntity entity)
        {
            var result = _context.Set<TEntity>()
                .Add(entity).Entity;

            return result;
        }

        public virtual IEnumerable<TEntity> Post(IEnumerable<TEntity> entities)
        {
            var results = new List<TEntity>();

            foreach (var entity in entities)
            {
                var result = _context.Set<TEntity>()
                    .Add(entity).Entity;

                results.Add(result);
            }

            return results;
        }

        public virtual TEntity Delete(TEntity entity)
        {
            var result = _context.Set<TEntity>()
                .Remove(entity).Entity;

            return result;
        }

        public virtual IEnumerable<TEntity> Delete(IEnumerable<TEntity> entities)
        {
            var results = new List<TEntity>();

            foreach (var entity in entities)
            {
                var result = _context.Set<TEntity>()
                    .Remove(entity).Entity;

                results.Add(result);
            }

            return results;
        }

        public virtual IEnumerable<TEntity> Delete(LambdaExpression lambda)
        {
            var entities = _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .ToList();

            return Delete(entities);
        }

        public virtual int DeleteBulk(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>()
                .Where(predicate)
                .ExecuteDelete();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public virtual bool Exists(LambdaExpression lambda)
        {
            return _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .Any();
        }

        public virtual TEntity Find(params object[] keyValues)
        {
            return _context.Set<TEntity>().Find(keyValues);
        }

        public virtual IEnumerable<TEntity> Get(
            IEnumerable<Expression<Func<TEntity, object>>> expressions)
        {
            return _context.Set<TEntity>().AsQueryable()
                .Include(expressions)
                .ToList();
        }

        public virtual IEnumerable<TEntity> Get(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null)
        {
            return _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .Include(expressions)
                .ToList();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicates,
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

            return query.ToList();
        }

        public virtual IEnumerable<TEntity> GetAsNoTracking(
            IEnumerable<Expression<Func<TEntity, object>>> expressions)
        {
            return _context.Set<TEntity>().AsQueryable()
                .Include(expressions)
                .AsNoTracking()
                .ToList();
        }

        public virtual IEnumerable<TEntity> GetAsNoTracking(
            LambdaExpression lambda = null,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null)
        {
            return _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .Include(expressions)
                .AsNoTracking()
                .ToList();
        }

        public virtual IEnumerable<TEntity> GetAsNoTracking(Expression<Func<TEntity, bool>> predicates,
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

            return query.AsNoTracking().ToList();
        }

        public virtual TEntity Put(TEntity entity)
        {
            var result = _context.Set<TEntity>()
                .Update(entity).Entity;

            return result;
        }

        public virtual IEnumerable<TEntity> Put(IEnumerable<TEntity> entities)
        {
            var results = new List<TEntity>();

            foreach (var entity in entities)
            {
                var result = _context.Set<TEntity>()
                    .Update(entity).Entity;

                results.Add(result);
            }

            return results;
        }

        public virtual int UpdateBulk(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls)
        {
            return _context.Set<TEntity>()
                .Where(predicate)
                .ExecuteUpdate(setPropertyCalls);
        }

        public virtual IQueryable<TEntity> Query(bool asNoTracking = true)
        {
            return asNoTracking
                ? _context.Set<TEntity>().AsNoTracking()
                : _context.Set<TEntity>().AsQueryable();
        }

        public virtual IDataStateResult<TEntity> Get(
            IDataState state,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null,
            bool asNoTracking = true)
        {
            var query = asNoTracking
                ? _context.Set<TEntity>().AsNoTracking()
                : _context.Set<TEntity>().AsQueryable();

            if (includes != null)
                query = includes(query);

            // Get total count before paging (apply only filter predicate)
            var countLambda = QueryExpressionFactory.GetQueryExpression<TEntity>()
                .ApplyPredicate(state);
            var total = countLambda != null
                ? query.Compile(countLambda).Count()
                : query.Count();

            // Apply full state (filter + sort + paging)
            var stateLambda = QueryExpressionFactory.GetQueryExpression<TEntity>()
                .ApplyState(state);
            var data = query.Compile(stateLambda).ToList();

            return new PagerStateResult<TEntity> { Data = data, Total = total };
        }

        public virtual IEnumerable<TResult> Select<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            bool asNoTracking = true)
        {
            var query = asNoTracking
                ? _context.Set<TEntity>().AsNoTracking()
                : _context.Set<TEntity>().AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            return query.Select(selector).ToList();
        }
    }
}
