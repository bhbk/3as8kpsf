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
            var result = (lambda != null)
                ? (IQueryable)lambda.Compile().DynamicInvoke(query)
                : query;

            return result.OfType<TEntity>();
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
}
