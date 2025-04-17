using Bhbk.Lib.QueryExpression.Exceptions;
using Bhbk.Lib.QueryExpression.Factories;
using System;
using System.Linq;
using System.Reflection;

namespace Bhbk.Lib.QueryExpression.Helpers
{
    public static class QueryExpressionHelper
    {
        /// <summary>
        /// Creates a PropertyInfo object from the specified typeparam and parameter. In the case
        /// of a delimited or compound parameter, traverse the hierarchy to get the last child
        /// segment.
        /// </summary>
        public static PropertyInfo GetPropertyInfo<TEntity>(string field)
        {
            Type entityType = typeof(TEntity);

            PropertyInfo propertyInfo = null;

            field.Split('.').ToList()
                .ForEach(segment =>
                {
                    propertyInfo = (propertyInfo == null)
                        ? entityType.GetProperty(
                            segment, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        : propertyInfo.PropertyType.GetProperty(
                            segment, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                });

            return propertyInfo
                ?? throw new QueryExpressionPropertyException(entityType.Name, field);
        }
    }
}
