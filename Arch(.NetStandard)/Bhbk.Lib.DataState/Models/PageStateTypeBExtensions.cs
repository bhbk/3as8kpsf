using Bhbk.Lib.DataState.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bhbk.Lib.DataState.Expressions
{
    public static class PageStateTypeBExtensions
    {
        public static LambdaExpression ToPredicateExpression<TEntity>(
            this PageStateTypeB state)
        {
            var expression = new QueryExpression<TEntity>();

            return expression.ToLambda();
        }

        public static LambdaExpression ToExpression<TEntity>(
            this PageStateTypeB state)
        {
            var expression = new QueryExpression<TEntity>();

            if (!IsSortValid(state.Sort))
                throw new QueryExpressionSortException($"The value for sort is invalid.");

            if (state.Skip < 0)
                throw new QueryExpressionSkipException(state.Skip);

            if (state.Take < 1)
                throw new QueryExpressionTakeException(state.Take);

            string method = string.Empty;

            foreach (var orderBy in state.Sort)
            {
                if (method == string.Empty)
                    method = orderBy.Value == "asc" ? "OrderBy" : "OrderByDescending";
                else
                    method = orderBy.Value == "asc" ? "ThenBy" : "ThenByDescending";

                expression = expression.OrderBy(method, orderBy.Key);
            }

            expression = expression.Skip(state.Skip);
            expression = expression.Take(state.Take);

            return expression.ToLambda();
        }

        internal static bool IsSortValid(List<KeyValuePair<string, string>> sort)
        {
            if (sort == null
                || sort.Count == 0
                || sort.Any(x => string.IsNullOrEmpty(x.Key))
                || sort.Any(x => !x.Value.Equals("asc") && !x.Value.Equals("desc")))
                return false;

            return true;
        }
    }
}
