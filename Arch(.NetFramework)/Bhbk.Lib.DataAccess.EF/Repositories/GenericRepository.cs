using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EF.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Bhbk.Lib.DataAccess.EF.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>, IDisposable
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
                .Add(entity);

            return result;
        }

        public virtual IEnumerable<TEntity> Create(IEnumerable<TEntity> entities)
        {
            var results = new List<TEntity>();

            foreach (var entity in entities)
            {
                var result = _context.Set<TEntity>()
                    .Add(entity);

                results.Add(result);
            }

            return results;
        }

        public virtual TEntity Delete(TEntity entity)
        {
            var result = _context.Set<TEntity>()
                .Remove(entity);

            return result;
        }

        public virtual IEnumerable<TEntity> Delete(IEnumerable<TEntity> entities)
        {
            var results = new List<TEntity>();

            foreach (var entity in entities)
            {
                var result = _context.Set<TEntity>()
                    .Remove(entity);

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

        public virtual TEntity Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            return _context.Entry(entity).Entity;
        }

        public virtual IEnumerable<TEntity> Update(IEnumerable<TEntity> entities)
        {
            var results = new List<TEntity>();

            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Modified;

                var result = _context.Entry(entity).Entity;

                results.Add(result);
            }

            return results;
        }
    }
}
