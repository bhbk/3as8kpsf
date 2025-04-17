using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EF.Extensions;
using Bhbk.Lib.DataAccess.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Bhbk.Lib.DataAccess.EF.UnitOfWorks
{
    public abstract class GenericUnitOfWork<TContext> : IGenericUnitOfWork
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private readonly InstanceContext _instanceType;
        private bool _disposed;

        public virtual InstanceContext InstanceType => _instanceType;

        public DbContext DbContext => _context;

        protected TContext Context => _context;

        protected GenericUnitOfWork(TContext context, InstanceContext instanceType)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _instanceType = instanceType;
            _repositories = new Dictionary<Type, object>();
        }

        public virtual IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var entityType = typeof(TEntity);

            if (!_repositories.TryGetValue(entityType, out var repository))
            {
                repository = new GenericRepository<TEntity>(_context);
                _repositories[entityType] = repository;
            }

            return (IGenericRepository<TEntity>)repository;
        }

        protected virtual void ValidateEntities()
        {
            _context.ValidateEntities();
        }

        public virtual void Commit(int? timeout = null)
        {
            if (timeout.HasValue)
                _context.Database.SetCommandTimeout(timeout.Value);

            ValidateEntities();
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
                _disposed = true;
            }
        }
    }
}
