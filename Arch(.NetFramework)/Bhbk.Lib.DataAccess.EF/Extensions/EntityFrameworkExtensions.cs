using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Bhbk.Lib.DataAccess.EF.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public static IQueryable<TEntity> Compile<TEntity>(
            this IQueryable<TEntity> query, LambdaExpression lambda)
            where TEntity : class
        {
            if (lambda == null)
                return query;

            var result = lambda.Compile().DynamicInvoke(query);

            try
            {
                return (IQueryable<TEntity>)result;
            }
            catch (Exception)
            {
                throw new EntityFrameworkExtensionCastException(
                    $"The entity: \"{result.GetType().ToString()}\" can not be cast to: \"{typeof(IQueryable<TEntity>).ToString()}\".");
            }
        }

        public static IQueryable<TEntity> Include<TEntity>(
            this IQueryable<TEntity> query,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null)
            where TEntity : class
        {
            if (expressions != null)
                foreach (var expression in expressions)
                    query = query.Include(expression);

            return query;
        }
    }

    public class EntityFrameworkExtensionException : Exception
    {
        public EntityFrameworkExtensionException(string message)
            : base(message) { }

        public EntityFrameworkExtensionException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class EntityFrameworkExtensionCastException : EntityFrameworkExtensionException
    {
        public EntityFrameworkExtensionCastException(string message)
            : base(message) { }
    }
}
