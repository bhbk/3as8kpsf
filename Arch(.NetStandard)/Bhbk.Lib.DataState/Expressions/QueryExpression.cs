using System;
using System.Linq.Expressions;

namespace Bhbk.Lib.DataState.Expressions
{
    public class QueryExpression<TEntity>
    {
        public Expression Body { get; set; }
        public ParameterExpression Param { get; set; }

        public QueryExpression(string param = null)
        {
            Body = null;
            Param = QueryExpressionHelpers.GetQueryParameter<TEntity>(param);
        }
    }

    public class QueryExpressionException : Exception
    {
        public QueryExpressionException(string message)
            : base(message) { }

        public QueryExpressionException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class QueryExpressionFilterException : QueryExpressionException
    {
        public QueryExpressionFilterException(string message)
            : base(message) { }
    }

    public class QueryExpressionSortException : QueryExpressionException
    {
        public QueryExpressionSortException(string message)
            : base(message) { }
    }

    public class QueryExpressionSkipException : QueryExpressionException
    {
        public QueryExpressionSkipException(int skip)
            : base($"The value for skip: \"{skip}\" is invalid.") { }
    }

    public class QueryExpressionTakeException : QueryExpressionException
    {
        public QueryExpressionTakeException(int take)
            : base($"The value for take: \"{take}\" is invalid.") { }
    }

    public class QueryExpressionPropertyException : QueryExpressionException
    {
        public QueryExpressionPropertyException(string entity, string field)
            : base($"The entity: \"{entity}\" does not contain field: \"{field}\".") { }
    }
}
