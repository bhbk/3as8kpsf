using Bhbk.Lib.DataState.Models;
using System.Linq;
using System.Linq.Expressions;

namespace Bhbk.Lib.DataState.Expressions
{
    public static class CascadePagerExtensions
    {
        public static LambdaExpression ToPredicateExpression<TEntity>(this CascadePager state)
        {
            QueryExpression<TEntity> expression = new QueryExpression<TEntity>();

            return expression.ToLambda();
        }

        public static LambdaExpression ToExpression<TEntity>(this CascadePager state)
        {
            QueryExpression<TEntity> expression = new QueryExpression<TEntity>();

            if (state.Skip < 0)
                throw new QueryExpressionSkipException(state.Skip);

            if (state.Take < 1)
                throw new QueryExpressionTakeException(state.Take);

            if (state.Sort == null 
                || state.Sort.Count == 0
                || state.Sort.Any(x => string.IsNullOrEmpty(x.Key))
                || state.Sort.Any(x => !x.Value.Equals("asc") && !x.Value.Equals("desc")))
                throw new QueryExpressionSortException($"The value for sort is invalid.");

            string method = string.Empty;

            foreach (var orderBy in state.Sort)
            {
                if (method == string.Empty)
                    method = orderBy.Value == "desc" ? "OrderByDescending" : "OrderBy";
                else
                    method = orderBy.Value == "desc" ? "ThenByDescending" : "ThenBy";

                expression = expression.OrderBy(method, orderBy.Key);
            }

            expression = expression.Skip(state.Skip);
            expression = expression.Take(state.Take);

            return expression.ToLambda();
        }
    }
}
