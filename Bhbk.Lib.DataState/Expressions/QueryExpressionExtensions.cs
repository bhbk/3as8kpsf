using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

/*
 * https://expressiontree-tutorial.net/
 * https://entityframework-extensions.net/
 */

namespace Bhbk.Lib.DataState.Expressions
{
    public static partial class QueryExpressionExtensions
    {
        public static QueryExpression<TEntity> First<TEntity>(
            this QueryExpression<TEntity> query)
        {
            MethodInfo method = typeof(Queryable).GetMethods().Where(x => x.Name == "First")
              .Single(x => x.GetParameters().Length == 1)
              .MakeGenericMethod(typeof(TEntity));

            query.Body = Expression.Call(
                method,
                query.Body ?? query.Param);

            //query.Body = Expression.Call(
            //    typeof(Queryable),
            //    "First",
            //    new Type[] { typeof(TEntity) },
            //    query.Body ?? query.Param);

            return query;
        }

        public static QueryExpression<TEntity> First<TEntity>(
            this QueryExpression<TEntity> query, Expression<Func<TEntity, bool>> expression)
        {
            MethodInfo method = typeof(Queryable).GetMethods().Where(x => x.Name == "First")
              .Single(x => x.GetParameters().Length == 2)
              .MakeGenericMethod(typeof(TEntity));

            query.Body = Expression.Call(
                method,
                query.Body ?? query.Param,
                expression);

            //query.Body = Expression.Call(
            //    typeof(Queryable),
            //    "First",
            //    new Type[] { typeof(TEntity) },
            //    query.Body ?? query.Param,
            //    expression);

            return query;
        }

        public static QueryExpression<TEntity> Last<TEntity>(
            this QueryExpression<TEntity> query)
        {
            MethodInfo method = typeof(Queryable).GetMethods().Where(x => x.Name == "Last")
              .Single(x => x.GetParameters().Length == 1)
              .MakeGenericMethod(typeof(TEntity));

            query.Body = Expression.Call(
                method,
                query.Body ?? query.Param);

            return query;
        }

        public static QueryExpression<TEntity> Last<TEntity>(
            this QueryExpression<TEntity> query, Expression<Func<TEntity, bool>> expression)
        {
            MethodInfo method = typeof(Queryable).GetMethods().Where(x => x.Name == "Last")
              .Single(x => x.GetParameters().Length == 2)
              .MakeGenericMethod(typeof(TEntity));

            query.Body = Expression.Call(
                method,
                query.Body ?? query.Param,
                expression);

            return query;
        }

        public static QueryExpression<TEntity> OrderBy<TEntity>(
            this QueryExpression<TEntity> query, string field) =>
                query.OrderBy("OrderBy", field);

        public static QueryExpression<TEntity> OrderBy<TEntity>(
            this QueryExpression<TEntity> query, string method, string field)
        {
            Type entityType = typeof(TEntity);
            ParameterExpression classParam = QueryExpressionHelpers.GetObjectParameter<TEntity>("x");
            PropertyInfo propertyInfo = entityType.GetProperty(
                field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
                throw new QueryExpressionPropertyException(entityType.Name, field);

            MemberExpression propertyExpr = Expression.Property(classParam, propertyInfo);
            Type propertyType = propertyInfo.PropertyType;

            query.Body = Expression.Call(
                typeof(Queryable),
                method,
                new Type[] { entityType, propertyType },
                query.Body ?? query.Param,
                Expression.Lambda(propertyExpr, classParam));

            return query;
        }

        public static QueryExpression<TEntity> OrderByDescending<TEntity>(
            this QueryExpression<TEntity> query, string field) =>
                query.OrderBy("OrderByDescending", field);

        public static QueryExpression<TEntity> Skip<TEntity>(
            this QueryExpression<TEntity> query, int skip)
        {
            if (skip < 0)
                throw new QueryExpressionSkipException(skip);

            MethodInfo method = typeof(Queryable).GetMethod("Skip")
                .MakeGenericMethod(typeof(TEntity));

            query.Body = Expression.Call(
                method,
                query.Body ?? query.Param,
                Expression.Constant(skip));

            return query;
        }

        public static QueryExpression<TEntity> Skip<TEntity>(
            this QueryExpression<TEntity> query, int skip, int take)
        {
            if (skip < 0)
                throw new QueryExpressionSkipException(skip);

            if (take < 1)
                throw new QueryExpressionTakeException(take);

            MethodInfo method = typeof(Queryable).GetMethod("Skip")
                .MakeGenericMethod(typeof(TEntity));

            query.Body = Expression.Call(
                method,
                query.Body ?? query.Param,
                Expression.Constant(skip * take));

            return query;
        }

        public static QueryExpression<TEntity> Take<TEntity>(
            this QueryExpression<TEntity> query, int take)
        {
            if (take < 1)
                throw new QueryExpressionTakeException(take);

            MethodInfo method = typeof(Queryable).GetMethod("Take")
                .MakeGenericMethod(typeof(TEntity));

            query.Body = Expression.Call(
                method,
                query.Body ?? query.Param,
                Expression.Constant(take));

            return query;
        }

        public static QueryExpression<TEntity> ThenBy<TEntity>(
            this QueryExpression<TEntity> query, string field) =>
                query.OrderBy("ThenBy", field);

        public static QueryExpression<TEntity> ThenByDescending<TEntity>(
            this QueryExpression<TEntity> query, string field) =>
                query.OrderBy("ThenByDescending", field);

        public static QueryExpression<TEntity> Where<TEntity>(
            this QueryExpression<TEntity> query, Expression<Func<TEntity, bool>> expression)
        {
            query.Body = Expression.Call(
                typeof(Queryable),
                "Where",
                new Type[] { typeof(TEntity) },
                query.Body ?? query.Param,
                expression);

            return query;
        }

        public static LambdaExpression ToLambda<TEntity>(
            this QueryExpression<TEntity> query)
        {
            if (query.Body != null)
                return Expression.Lambda(
                    query.Body,
                    query.Param);
            else
                return null;
        }
    }
}
