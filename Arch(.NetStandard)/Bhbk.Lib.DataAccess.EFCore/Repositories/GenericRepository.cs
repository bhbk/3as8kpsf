using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EFCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bhbk.Lib.DataAccess.EFCore.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected readonly InstanceContext _instance;
        protected readonly DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context ?? throw new NullReferenceException();
            _instance = InstanceContext.DeployedOrLocal;
        }

        public GenericRepository(DbContext context, InstanceContext instance)
        {
            _context = context ?? throw new NullReferenceException();
            _instance = instance;
        }

        public virtual int Count(LambdaExpression lambda = null)
        {
            return _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .Count();
        }

        public virtual TEntity Create(TEntity entity)
        {
            var result = _context.Set<TEntity>()
                .Add(entity).Entity;

            return result;
        }

        public virtual IEnumerable<TEntity> Create(IEnumerable<TEntity> entities)
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

        public virtual bool Exists(LambdaExpression lambda)
        {
            return _context.Set<TEntity>().AsQueryable()
                .Compile(lambda)
                .Any();
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

        [Obsolete]
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

        public virtual TEntity Update(TEntity entity)
        {
            var result = _context.Set<TEntity>()
                .Update(entity).Entity;

            return result;
        }

        public virtual IEnumerable<TEntity> Update(IEnumerable<TEntity> entities)
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
    }
}
