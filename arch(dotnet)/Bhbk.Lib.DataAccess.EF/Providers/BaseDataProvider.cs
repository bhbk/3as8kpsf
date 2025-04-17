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

namespace Bhbk.Lib.DataAccess.EF.Providers
{
    public abstract class BaseDataProvider<TEntity> : IBaseDataProvider<TEntity>
        where TEntity : class
    {
        private readonly IGenericUnitOfWork _unitOfWork;
        private readonly DbContext _context;
        private readonly IGenericRepository<TEntity> _repository;
        private readonly bool _ownsContext;
        private bool _disposed;

        protected IGenericRepository<TEntity> Repository => _repository;
        protected IGenericUnitOfWork UnitOfWork => _unitOfWork;
        protected DbContext Context => _context;

        protected BaseDataProvider(IGenericUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _context = unitOfWork.DbContext;
            _repository = unitOfWork.Repository<TEntity>();
            _ownsContext = false;
        }

        protected BaseDataProvider(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repository = new GenericRepository<TEntity>(context);
            _ownsContext = true;
        }

        #region Query

        public virtual IQueryable<TEntity> Query(bool asNoTracking = true)
        {
            return _repository.Query(asNoTracking);
        }

        #endregion

        #region Read

        public virtual TEntity Find(params object[] keyValues)
        {
            return _repository.Find(keyValues);
        }

        public virtual IEnumerable<TEntity> Select(
            Expression<Func<TEntity, bool>> predicate = null,
            bool asNoTracking = true,
            IEnumerable<Expression<Func<TEntity, object>>> includes = null)
        {
            if (predicate == null)
            {
                return asNoTracking
                    ? _repository.GetAsNoTracking(includes)
                    : _repository.Get(includes);
            }

            var lambda = QueryExpressionFactory.GetQueryExpression<TEntity>()
                .Where(predicate)
                .ToLambda();

            return asNoTracking
                ? _repository.GetAsNoTracking(lambda, includes)
                : _repository.Get(lambda, includes);
        }

        public virtual IDataStateResult<TEntity> Select(
            IDataState state,
            bool asNoTracking = true,
            IEnumerable<Expression<Func<TEntity, object>>> includes = null)
        {
            return _repository.Get(state, includes: null, asNoTracking);
        }

        public virtual IEnumerable<TResult> Select<TResult>(
            Expression<Func<TEntity, TResult>> selector,
            Expression<Func<TEntity, bool>> predicate = null,
            bool asNoTracking = true)
        {
            return _repository.Select(selector, predicate, asNoTracking);
        }

        #endregion

        #region Post

        public virtual TEntity Post(TEntity entity, bool commit = true)
        {
            var result = _repository.Post(entity);

            if (commit)
                CommitInternal();

            return result;
        }

        #endregion

        #region Put

        public virtual TEntity Put(TEntity entity, bool commit = true)
        {
            var result = _repository.Put(entity);

            if (commit)
                CommitInternal();

            return result;
        }

        #endregion

        #region Delete

        public virtual void Delete(TEntity entity, bool commit = true)
        {
            _repository.Delete(entity);

            if (commit)
                CommitInternal();
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate, bool commit = true)
        {
            var lambda = QueryExpressionFactory.GetQueryExpression<TEntity>()
                .Where(predicate)
                .ToLambda();

            _repository.Delete(lambda);

            if (commit)
                CommitInternal();
        }

        #endregion

        #region Commit Helper

        private void CommitInternal()
        {
            if (_unitOfWork != null)
                _unitOfWork.Commit();
            else
                _context.SaveChanges();
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_ownsContext)
                    _context.Dispose();

                _disposed = true;
            }
        }

        #endregion
    }
}
