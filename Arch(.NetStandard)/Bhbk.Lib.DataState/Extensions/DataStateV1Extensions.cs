using Bhbk.Lib.DataState.Interfaces;
using Bhbk.Lib.QueryExpression;
using Bhbk.Lib.QueryExpression.Exceptions;
using Bhbk.Lib.QueryExpression.Extensions;
using Bhbk.Lib.QueryExpression.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bhbk.Lib.DataState.Extensions
{
    public static class DataStateV1Extensions
    {
        private static Expression GetPredicateExpression<TEntity>(
            IDataStateFilter filter,
            ParameterExpression parameter)
        {
            Expression predicate = null;

            if (filter != null
                && filter.Filters != null
                && filter.Filters.Count != 0)
            {
                var logic = filter.Logic;

                foreach (var entry in filter.Filters)
                {
                    var expression = GetPredicateExpression<TEntity>(entry, parameter);

                    if (predicate == null)
                        predicate = expression;
                    else
                    {
                        if (logic == "and")
                            predicate = Expression.And(predicate, expression);
                        else
                            predicate = Expression.Or(predicate, expression);
                    }
                }
            }
            else if (filter != null)
            {
                predicate = ExpressionFactory.GetMethodExpression<TEntity>(
                    parameter, filter.Field, filter.Operator, filter.Value);
            }

            return predicate;
        }

        public static LambdaExpression ApplyPredicate<TEntity>(
            this IQueryExpression<TEntity> query, IDataState state)
        {
            if (IsFilterValid(state.Filter))
            {
                var parameter = ExpressionFactory.GetObjectParameter<TEntity>("q");

                var predicate = Expression.Lambda<Func<TEntity, bool>>(
                    GetPredicateExpression<TEntity>(state.Filter, parameter),
                    parameter);

                query = query.Where(predicate);
            }

            return query.ToLambda();
        }

        public static LambdaExpression ApplyState<TEntity>(
            this IQueryExpression<TEntity> query, IDataState state)
        {
            if (!IsSortValid(state.Sort))
                throw new QueryExpressionSortException($"The value for sort is invalid.");

            if (state.Skip < 0)
                throw new QueryExpressionSkipException(state.Skip);

            if (state.Take < 1)
                throw new QueryExpressionTakeException(state.Take);

            if (IsFilterValid(state.Filter))
            {
                var parameter = ExpressionFactory.GetObjectParameter<TEntity>("q");

                var predicate = Expression.Lambda<Func<TEntity, bool>>(
                    GetPredicateExpression<TEntity>(state.Filter, parameter),
                    parameter);

                query = query.Where(predicate);
            }

            string method = string.Empty;

            foreach (var orderBy in state.Sort)
            {
                if (string.IsNullOrEmpty(orderBy.Dir))
                    orderBy.Dir = "asc";

                if (method == string.Empty)
                    method = orderBy.Dir == "asc" ? "OrderBy" : "OrderByDescending";
                else
                    method = orderBy.Dir == "asc" ? "ThenBy" : "ThenByDescending";

                query = query.OrderBy(method, orderBy.Field);
            }

            query = query.Skip(state.Skip);
            query = query.Take(state.Take);

            return query.ToLambda();
        }

        internal static bool IsFilterValid(IDataStateFilter filter)
        {
            if (filter != null
                && filter.Filters != null
                && filter.Filters.Count() != 0)
                return true;

            return false;
        }

        internal static bool IsSortValid(ICollection<IDataStateSort> sort)
        {
            if (sort == null
                || sort.Count == 0
                || sort.Any(x => string.IsNullOrEmpty(x.Field))
                || !sort.All(x => string.IsNullOrEmpty(x.Dir) 
                    || x.Dir.Equals("asc") 
                    || x.Dir.Equals("desc")))
                return false;

            return true;
        }
    }
}
