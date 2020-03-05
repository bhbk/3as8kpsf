using Bhbk.Lib.QueryExpression.Exceptions;
using Bhbk.Lib.QueryExpression.Factories;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

/*
 * https://expressiontree-tutorial.net/
 * https://entityframework-extensions.net/
 */

namespace Bhbk.Lib.QueryExpression.Extensions
{
    public static partial class QueryExpressionExtensions
    {
        public static IQueryExpression<TEntity> First<TEntity>(
            this IQueryExpression<TEntity> query)
        {
            throw new NotImplementedException();

            var method = typeof(Queryable).GetMethods()
                .Where(x => x.Name == "First")
                .Single(x => x.GetParameters().Length == 1).MakeGenericMethod(typeof(TEntity));

            query.Body = Expression.Call(
                method,
                query.Body ?? query.Param);

            return query;
        }

        public static IQueryExpression<TEntity> First<TEntity>(
            this IQueryExpression<TEntity> query, Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();

            var method = typeof(Queryable).GetMethods()
                .Where(x => x.Name == "First")
                .Single(x => x.GetParameters().Length == 2).MakeGenericMethod(typeof(TEntity));

            query.Body = Expression.Call(
                method,
                query.Body ?? query.Param,
                expression);

            return query;
        }

        public static IQueryExpression<TEntity> Last<TEntity>(
            this IQueryExpression<TEntity> query)
        {
            throw new NotImplementedException();

            var method = typeof(Queryable).GetMethods()
                .Where(x => x.Name == "Last")
                .Single(x => x.GetParameters().Length == 1).MakeGenericMethod(typeof(TEntity));

            query.Body = Expression.Call(
                method,
                query.Body ?? query.Param);

            return query;
        }

        public static IQueryExpression<TEntity> Last<TEntity>(
            this IQueryExpression<TEntity> query, Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();

            var method = typeof(Queryable).GetMethods()
                .Where(x => x.Name == "Last")
                .Single(x => x.GetParameters().Length == 2).MakeGenericMethod(typeof(TEntity));

            query.Body = Expression.Call(
                method,
                query.Body ?? query.Param,
                expression);

            return query;
        }

        public static IQueryExpression<TEntity> OrderBy<TEntity>(
            this IQueryExpression<TEntity> query, string field) =>
                query.OrderBy("OrderBy", field);

        public static IQueryExpression<TEntity> OrderBy<TEntity>(
            this IQueryExpression<TEntity> query, string method, string field)
        {
            var entityType = typeof(TEntity);
            var classParam = ExpressionFactory.GetObjectParameter<TEntity>("p");
            var propertyInfo = entityType.GetProperty(
                field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
                throw new QueryExpressionPropertyException(entityType.Name, field);

            var propertyExpr = Expression.Property(classParam, propertyInfo);
            var propertyType = propertyInfo.PropertyType;

            query.Body = Expression.Call(
                typeof(Queryable),
                method,
                new Type[] { entityType, propertyType },
                query.Body ?? query.Param,
                Expression.Lambda(propertyExpr, classParam));

            return query;
        }

        public static IQueryExpression<TEntity> OrderBy<TEntity>(
            this IQueryExpression<TEntity> query, Expression<Func<TEntity, bool>> expression)
        {
            var method = typeof(Queryable).GetMethods()
                .Where(x => x.Name == "OrderBy")
                .Select(x => new { overloads = x, parameters = x.GetParameters() })
                .Where(x => x.parameters.Length == 2
                    && x.parameters[0].ParameterType.IsGenericType
                    && x.parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>)
                    && x.parameters[1].ParameterType.IsGenericType
                    && x.parameters[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>))
                .Select(x => new { x.overloads, genericArgs = x.parameters[1].ParameterType.GetGenericArguments() })
                .Where(x => x.genericArgs[0].IsGenericType
                    && x.genericArgs[0].GetGenericTypeDefinition() == typeof(Func<,>))
                .Select(x => new { x.overloads, genericArgs = x.genericArgs[0].GetGenericArguments() })
                .Where(x => x.genericArgs[0].IsGenericParameter
                    && x.genericArgs[1] == typeof(bool))
                .Select(x => x.overloads)
                .Single().MakeGenericMethod(typeof(TEntity));

            query.Body = Expression.Call(
                method,
                query.Body ?? query.Param,
                expression);

            return query;
        }

        public static IQueryExpression<TEntity> OrderByDescending<TEntity>(
            this IQueryExpression<TEntity> query, string field) =>
                query.OrderBy("OrderByDescending", field);

        public static IQueryExpression<TEntity> Skip<TEntity>(
            this IQueryExpression<TEntity> query, int skip)
        {
            if (skip < 0)
                throw new QueryExpressionSkipException(skip);

            var method = typeof(Queryable).GetMethod("Skip")
                .MakeGenericMethod(typeof(TEntity));

            query.Body = Expression.Call(
                method,
                query.Body ?? query.Param,
                Expression.Constant(skip));

            return query;
        }

        public static IQueryExpression<TEntity> Skip<TEntity>(
            this IQueryExpression<TEntity> query, int skip, int take)
        {
            if (skip < 0)
                throw new QueryExpressionSkipException(skip);

            if (take < 1)
                throw new QueryExpressionTakeException(take);

            var method = typeof(Queryable).GetMethod("Skip")
                .MakeGenericMethod(typeof(TEntity));

            query.Body = Expression.Call(
                method,
                query.Body ?? query.Param,
                Expression.Constant(skip * take));

            return query;
        }

        public static IQueryExpression<TEntity> Take<TEntity>(
            this IQueryExpression<TEntity> query, int take)
        {
            if (take < 1)
                throw new QueryExpressionTakeException(take);

            var method = typeof(Queryable).GetMethod("Take")
                .MakeGenericMethod(typeof(TEntity));

            query.Body = Expression.Call(
                method,
                query.Body ?? query.Param,
                Expression.Constant(take));

            return query;
        }

        public static IQueryExpression<TEntity> ThenBy<TEntity>(
            this IQueryExpression<TEntity> query, string field) =>
                query.OrderBy("ThenBy", field);

        public static IQueryExpression<TEntity> ThenByDescending<TEntity>(
            this IQueryExpression<TEntity> query, string field) =>
                query.OrderBy("ThenByDescending", field);

        public static IQueryExpression<TEntity> Where<TEntity>(
            this IQueryExpression<TEntity> query, Expression<Func<TEntity, bool>> expression)
        {
            var method = typeof(Queryable).GetMethods()
                .Where(x => x.Name == "Where")
                .Select(x => new { overloads = x, parameters = x.GetParameters() })
                .Where(x => x.parameters.Length == 2
                    && x.parameters[0].ParameterType.IsGenericType
                    && x.parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>)
                    && x.parameters[1].ParameterType.IsGenericType
                    && x.parameters[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>))
                .Select(x => new { x.overloads, genericArgs = x.parameters[1].ParameterType.GetGenericArguments() })
                .Where(x => x.genericArgs[0].IsGenericType
                    && x.genericArgs[0].GetGenericTypeDefinition() == typeof(Func<,>))
                .Select(x => new { x.overloads, genericArgs = x.genericArgs[0].GetGenericArguments() })
                .Where(x => x.genericArgs[0].IsGenericParameter
                    && x.genericArgs[1] == typeof(bool))
                .Select(x => x.overloads)
                .Single().MakeGenericMethod(typeof(TEntity));

            query.Body = Expression.Call(
                method,
                query.Body ?? query.Param,
                expression);

            return query;
        }

        public static IQueryExpression<TEntity> Where<TEntity>(
            this IQueryExpression<TEntity> query, Expression<Func<TEntity, int, bool>> expression)
        {
            var method = typeof(Queryable).GetMethods()
                .Where(x => x.Name == "Where")
                .Select(x => new { overloads = x, parameters = x.GetParameters() })
                .Where(x => x.parameters.Length == 2
                    && x.parameters[0].ParameterType.IsGenericType
                    && x.parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IQueryable<>)
                    && x.parameters[1].ParameterType.IsGenericType
                    && x.parameters[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>))
                .Select(x => new { x.overloads, genericArgs = x.parameters[1].ParameterType.GetGenericArguments() })
                .Where(x => x.genericArgs[0].IsGenericType
                    && x.genericArgs[0].GetGenericTypeDefinition() == typeof(Func<,,>))
                .Select(x => new { x.overloads, genericArgs = x.genericArgs[0].GetGenericArguments() })
                .Where(x => x.genericArgs[0].IsGenericParameter
                    && x.genericArgs[1] == typeof(int)
                    && x.genericArgs[2] == typeof(bool))
                .Select(x => x.overloads)
                .Single().MakeGenericMethod(typeof(TEntity));

            query.Body = Expression.Call(
                method,
                query.Body ?? query.Param,
                expression);

            return query;
        }

        public static LambdaExpression ToLambda<TEntity>(
            this IQueryExpression<TEntity> query)
        {
            if (query.Body == null)
                return null;

            return Expression.Lambda(
                query.Body,
                query.Param);
        }
    }
}
