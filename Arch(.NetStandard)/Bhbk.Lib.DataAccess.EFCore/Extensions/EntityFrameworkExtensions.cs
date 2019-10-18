using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bhbk.Lib.DataAccess.EFCore.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public static IQueryable<TEntity> Compile<TEntity>(
            this IQueryable<TEntity> query, LambdaExpression lambda)
            where TEntity : class
        {
            //if (lambda == null)
            //    return query;

            //return query.Provider.CreateQuery<TEntity>(lambda);

            if (lambda == null)
                return query;

            return (IQueryable<TEntity>)lambda.Compile()
                .DynamicInvoke(query);
        }

        public static IQueryable<TEntity> Include<TEntity>(
            this IQueryable<TEntity> query,
            IEnumerable<Expression<Func<TEntity, object>>> expressions = null)
            where TEntity : class
        {
            if (expressions == null)
                return query;

            foreach (var expression in expressions)
                query = query.Include(expression);

            return query;
        }
    }
}
