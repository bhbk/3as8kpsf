using Bhbk.Lib.DataAccess.EF.Repositories;
using Bhbk.Lib.DataAccess.EF.UnitOfWorks;
using Bhbk.Lib.DataState.Interfaces;
using Bhbk.Lib.QueryExpression.Extensions;
using Bhbk.Lib.QueryExpression.Factories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataAccess.EF.Providers
{
    public abstract class BaseDataProviderAsync<TEntity> : IBaseDataProviderAsync<TEntity>
        where TEntity : class
    {
        private readonly IGenericUnitOfWorkAsync _unitOfWork;
        private readonly DbContext _context;
        private readonly IGenericRepositoryAsync<TEntity> _repository;
        private readonly bool _ownsContext;
        private bool _disposed;

        protected IGenericRepositoryAsync<TEntity> Repository => _repository;
        protected IGenericUnitOfWorkAsync UnitOfWork => _unitOfWork;
        protected DbContext Context => _context;

        protected BaseDataProviderAsync(IGenericUnitOfWorkAsync unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _context = unitOfWork.DbContext;
            _repository = unitOfWork.RepositoryAsync<TEntity>();
            _ownsContext = false;
        }

        protected BaseDataProviderAsync(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repository = new GenericRepositoryAsync<TEntity>(context);
            _ownsContext = true;
        }

        #region Query

        public virtual IQueryable<TEntity> Query(bool asNoTracking = true)
        {
            return _repository.Query(asNoTracking);
        }

        #endregion

        #region Read

        public virtual async ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken ct = default)
        {
            return await _repository.FindAsync(keyValues, ct);
        }

        public virtual async ValueTask<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            bool asNoTracking = true,
            IEnumerable<Expression<Func<TEntity, object>>> includes = null,
            CancellationToken ct = default)
        {
            if (predicate == null)
            {
                return asNoTracking
                    ? await _repository.GetAsNoTrackingAsync(includes, ct)
                    : await _repository.GetAsync(includes, ct);
            }

            var lambda = QueryExpressionFactory.GetQueryExpression<TEntity>()
                .Where(predicate)
                .ToLambda();

            return asNoTracking
                ? await _repository.GetAsNoTrackingAsync(lambda, includes, ct)
                : await _repository.GetAsync(lambda, includes, ct);
        }

        public virtual async ValueTask<IDataStateResult<TEntity>> SelectAsync(
            IDataState state,
            bool asNoTracking = true,
            IEnumerable<Expression<Func<TEntity, object>>> includes = null,
            CancellationToken ct = default)
        {
            return await _repository.GetAsync(state, includes: null, asNoTracking, ct);
        }

        public virtual async ValueTask<IEnumerable<TResult>> SelectAsync<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            bool asNoTracking = true,
            CancellationToken ct = default)
        {
            return await _repository.SelectAsync(selector, predicate, asNoTracking, ct);
        }

        #endregion

        #region Post

        public virtual async ValueTask<TEntity> PostAsync(TEntity entity, bool commit = true, CancellationToken ct = default)
        {
            var result = await _repository.PostAsync(entity, ct);

            if (commit)
                await CommitInternalAsync(ct);

            return result;
        }

        #endregion

        #region Put

        public virtual async ValueTask<TEntity> PutAsync(TEntity entity, bool commit = true, CancellationToken ct = default)
        {
            var result = await _repository.PutAsync(entity, ct);

            if (commit)
                await CommitInternalAsync(ct);

            return result;
        }

        #endregion

        #region Delete

        public virtual async ValueTask DeleteAsync(TEntity entity, bool commit = true, CancellationToken ct = default)
        {
            await _repository.DeleteAsync(entity, ct);

            if (commit)
                await CommitInternalAsync(ct);
        }

        public virtual async ValueTask DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool commit = true, CancellationToken ct = default)
        {
            var lambda = QueryExpressionFactory.GetQueryExpression<TEntity>()
                .Where(predicate)
                .ToLambda();

            await _repository.DeleteAsync(lambda, ct);

            if (commit)
                await CommitInternalAsync(ct);
        }

        #endregion

        #region Commit Helper

        private async Task CommitInternalAsync(CancellationToken ct = default)
        {
            if (_unitOfWork != null)
                await _unitOfWork.CommitAsync(ct);
            else
                await _context.SaveChangesAsync(ct);
        }

        #endregion

        #region Dispose

        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                if (_ownsContext)
                    await _context.DisposeAsync();

                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
