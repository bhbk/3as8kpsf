using Bhbk.Lib.DataState.Interfaces;
using Bhbk.Lib.QueryExpression;
using Bhbk.Lib.QueryExpression.Exceptions;
using Bhbk.Lib.QueryExpression.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bhbk.Lib.DataState.Extensions
{
    public static class PagerV2Extensions
    {
        public static LambdaExpression ApplyPredicate<TEntity>(
            this IQueryExpression<TEntity> query, IPager state)
        {
            return query.ToLambda();
        }

        public static LambdaExpression ApplyState<TEntity>(
            this IQueryExpression<TEntity> query, IPager state)
        {
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

                query = query.OrderBy(method, orderBy.Key);
            }

            query = query.Skip(state.Skip);
            query = query.Take(state.Take);

            return query.ToLambda();
        }

        internal static bool IsSortValid(ICollection<KeyValuePair<string, string>> sort)
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
