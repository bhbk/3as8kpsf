﻿using Bhbk.Lib.QueryExpression.Exceptions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bhbk.Lib.QueryExpression.Factories
{
    public static class ExpressionFactory
    {
        public static ConstantExpression GetConstantExpression<TEntity>(string field, string value)
        {
            var entityType = typeof(TEntity);

            var propertyInfo = entityType.GetProperty(
                field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
                throw new QueryExpressionPropertyException(entityType.Name, field);

            var propertyType = typeof(TEntity).GetProperties()
                .Single(p => p.Name.ToLower() == field?.ToLower())
                .PropertyType;

            switch (propertyType)
            {
                case Type type when propertyType == typeof(DateTime?):
                    return string.IsNullOrEmpty(value)
                        ? Expression.Constant(null, typeof(DateTime?))
                        : Expression.Constant(DateTime.Parse(value).Date, typeof(DateTime?));

                case Type type when propertyType == typeof(DateTime):
                    return Expression.Constant(DateTime.Parse(value).Date, typeof(DateTime));

                case Type type when propertyType == typeof(decimal?):
                    return Expression.Constant(value, typeof(decimal?));

                case Type type when propertyType == typeof(decimal):
                    return Expression.Constant(value, typeof(decimal));

                case Type type when propertyType == typeof(int?):
                    return Expression.Constant(Convert.ToInt32(value), typeof(int?));

                case Type type when propertyType == typeof(int):
                    return Expression.Constant(Convert.ToInt32(value), typeof(int));

                case Type type when propertyType == typeof(bool?):
                    return Expression.Constant(value, typeof(bool?));

                case Type type when propertyType == typeof(bool):
                    return Expression.Constant(value, typeof(bool));

                default:
                    return Expression.Constant(value, typeof(string));
            };
        }

        public static MemberExpression GetMemberExpression<TEntity>(
            ParameterExpression param, string field)
        {
            Expression navigation = param;

            try
            {
                field.Split('.').ToList()
                    .ForEach(segment => navigation = Expression.PropertyOrField(navigation, segment));
            }
            catch (ArgumentException)
            {
                throw new QueryExpressionPropertyException(param.Name, field);
            }

            return (MemberExpression)navigation;
        }

        public static Expression GetMethodExpression<TEntity>(
            ParameterExpression param, string field, string name, string value)
        {
            var constant = GetConstantExpression<TEntity>(field, value);
            var member = GetMemberExpression<TEntity>(param, field);
            MethodInfo method;

            switch (name.ToLower())
            {
                case "contains":
                    method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
                    return Expression.Call(member, method, constant);

                case "doesnotcontain":
                    method = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
                    return Expression.Not(Expression.Call(member, method, constant));

                case "endswith":
                    method = typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
                    return Expression.Call(member, method, constant);

                case "eq":
                case "equal":
                    return Expression.Equal(member, constant);

                case "gt":
                case "greaterthan":
                    return Expression.GreaterThan(member, constant);

                case "gte":
                case "greaterthanorequal":
                    return Expression.GreaterThanOrEqual(member, constant);

                case "isempty":
                case "isnull":
                case "isnullorempty":
                    method = typeof(string).GetMethod("IsNullOrEmpty", new Type[] { typeof(string) });
                    return Expression.Call(method, member);

                case "isnullorwhitespace":
                    method = typeof(string).GetMethod("IsNullOrWhiteSpace", new Type[] { typeof(string) });
                    return Expression.Call(method, member);

                case "lt":
                case "lessthan":
                    return Expression.LessThan(member, constant);

                case "lte":
                case "lessthanorequal":
                    return Expression.LessThanOrEqual(member, constant);

                case "isnotempty":
                case "isnotnull":
                case "neq":
                case "notequal":
                    return Expression.NotEqual(member, constant);

                case "startswith":
                    method = typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
                    return Expression.Call(member, method, constant);

                default:
                    throw new QueryExpressionFilterException(string.Format($"The operator: \"{name}\" is invalid."));
            };
        }

        public static ParameterExpression GetObjectParameter<TEntity>(string param)
        {
            return Expression.Parameter(typeof(TEntity), param);
        }

        public static PropertyInfo GetPropertyInfo<TEntity>(string field)
        {
            var entityType = typeof(TEntity);
            PropertyInfo propertyInfo = null;

            field.Split('.').ToList().ForEach(segment =>
            {
                propertyInfo = propertyInfo == null ?
                    entityType.GetProperty(segment, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) : 
                    propertyInfo.PropertyType.GetProperty(segment, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            });

            if (propertyInfo == null)
                throw new QueryExpressionPropertyException(entityType.Name, field);

            return propertyInfo;
        }

        public static ParameterExpression GetQueryParameter<TEntity>(string param = null)
        {
            return Expression.Parameter(typeof(IQueryable<TEntity>), param);
        }
    }
}
