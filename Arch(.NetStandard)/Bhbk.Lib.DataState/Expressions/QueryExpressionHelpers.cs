using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bhbk.Lib.DataState.Expressions
{
    public static class QueryExpressionHelpers
    {
        public static ConstantExpression GetConstantExpression<TEntity>(string field, string value)
        {
            Type entityType = typeof(TEntity);

            PropertyInfo propertyInfo = entityType.GetProperty(
                field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
                throw new QueryExpressionPropertyException(entityType.Name, field);

            Type propertyType = typeof(TEntity).GetProperties()
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

        public static MemberExpression GetMemberExpression<TEntity>(ParameterExpression param, string field)
        {
            PropertyInfo property =
                typeof(TEntity).GetProperties().Single(p => p.Name.ToLower() == field?.ToLower());

            return Expression.MakeMemberAccess(param, property);
        }

        public static Expression GetMethodExpression<TEntity>(
            ParameterExpression param, string field, string name, string value)
        {
            ConstantExpression constant = GetConstantExpression<TEntity>(field, value);
            MemberExpression member = GetMemberExpression<TEntity>(param, field);
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

        public static ParameterExpression GetQueryParameter<TEntity>(string param = null)
        {
            return Expression.Parameter(typeof(IQueryable<TEntity>), param);
        }
    }
}
