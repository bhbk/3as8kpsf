using System;

namespace Bhbk.Lib.QueryExpression.Exceptions
{
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
