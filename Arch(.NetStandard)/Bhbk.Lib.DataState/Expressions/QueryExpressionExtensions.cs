﻿using System;
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
        public static QueryExpression<TEntity> OrderBy<TEntity>(
            this QueryExpression<TEntity> query, string field) =>
                query.OrderBy("OrderBy", field);

        public static QueryExpression<TEntity> OrderBy<TEntity>(
            this QueryExpression<TEntity> query, string method, string field)
        {
            var entityType = typeof(TEntity);
            var classParam = QueryExpressionHelpers.GetObjectParameter<TEntity>("x");
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

        public static QueryExpression<TEntity> OrderBy<TEntity>(
            this QueryExpression<TEntity> query, Expression<Func<TEntity, bool>> expression)
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

        public static QueryExpression<TEntity> OrderByDescending<TEntity>(
            this QueryExpression<TEntity> query, string field) =>
                query.OrderBy("OrderByDescending", field);

        public static QueryExpression<TEntity> Skip<TEntity>(
            this QueryExpression<TEntity> query, int skip)
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

        public static QueryExpression<TEntity> Skip<TEntity>(
            this QueryExpression<TEntity> query, int skip, int take)
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

        public static QueryExpression<TEntity> Take<TEntity>(
            this QueryExpression<TEntity> query, int take)
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

        public static QueryExpression<TEntity> ThenBy<TEntity>(
            this QueryExpression<TEntity> query, string field) =>
                query.OrderBy("ThenBy", field);

        public static QueryExpression<TEntity> ThenByDescending<TEntity>(
            this QueryExpression<TEntity> query, string field) =>
                query.OrderBy("ThenByDescending", field);

        public static QueryExpression<TEntity> Where<TEntity>(
            this QueryExpression<TEntity> query, Expression<Func<TEntity, bool>> expression)
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

        public static QueryExpression<TEntity> Where<TEntity>(
            this QueryExpression<TEntity> query, Expression<Func<TEntity, int, bool>> expression)
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
            this QueryExpression<TEntity> query)
        {
            if (query.Body == null)
                return null;

            return Expression.Lambda(
                query.Body,
                query.Param);
        }
    }
}
