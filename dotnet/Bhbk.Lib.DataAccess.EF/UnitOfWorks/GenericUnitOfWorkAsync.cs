using Bhbk.Lib.Common.Primitives.Enums;
using Bhbk.Lib.DataAccess.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.UnitOfWorks
{
    public abstract class GenericUnitOfWorkAsync<TContext> : GenericUnitOfWork<TContext>, IGenericUnitOfWorkAsync
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly Dictionary<Type, object> _repositoriesAsync;
        private bool _disposed;

        protected GenericUnitOfWorkAsync(TContext context, InstanceContext instanceType)
            : base(context, instanceType)
        {
            _context = context;
            _repositoriesAsync = new Dictionary<Type, object>();
        }

        public virtual IGenericRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class
        {
            var entityType = typeof(TEntity);

            if (!_repositoriesAsync.TryGetValue(entityType, out var repository))
            {
                repository = new GenericRepositoryAsync<TEntity>(_context);
                _repositoriesAsync[entityType] = repository;
            }

            return (IGenericRepositoryAsync<TEntity>)repository;
        }

        public virtual async Task CommitAsync(CancellationToken ct = default)
        {
            ValidateEntities();
            await _context.SaveChangesAsync(ct);
        }

        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                await _context.DisposeAsync();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
