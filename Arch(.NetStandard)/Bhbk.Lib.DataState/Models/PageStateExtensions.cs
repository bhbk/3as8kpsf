﻿using Bhbk.Lib.DataState.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using static Bhbk.Lib.DataState.Models.PageState;

namespace Bhbk.Lib.DataState.Expressions
{
    public static class PageStateExtensions
    {
        public static LambdaExpression ToExpression<TEntity>(this PageState state)
        {
            QueryExpression<TEntity> expression = new QueryExpression<TEntity>();

            if (state.Skip < 0)
                throw new QueryExpressionSkipException(state.Skip);

            if (state.Take < 1)
                throw new QueryExpressionTakeException(state.Take);

            if (state.Sort == null
                || state.Sort.Count == 0
                || state.Sort.Any(x => string.IsNullOrEmpty(x.Field))
                || !state.Sort.All(x => string.IsNullOrEmpty(x.Dir) || x.Dir.Equals("asc") || x.Dir.Equals("desc")))
                throw new QueryExpressionSortException($"The value for sort is invalid.");

            if (state.Filter != null && state.Filter.Filters != null && state.Filter.Filters.Count() != 0)
            {
                ParameterExpression param = QueryExpressionHelpers.GetObjectParameter<TEntity>("q");

                var predicate = Expression.Lambda<Func<TEntity, bool>>(
                    GetPredicateExpression<TEntity>(state.Filter, param),
                    param);

                expression = expression.Where(predicate);
            }

            string method = string.Empty;

            foreach (var orderBy in state.Sort.Where(x => x.Dir != null))
            {
                if (method == string.Empty)
                    method = orderBy.Dir == "desc" ? "OrderByDescending" : "OrderBy";
                else
                    method = orderBy.Dir == "desc" ? "ThenByDescending" : "ThenBy";

                expression = expression.OrderBy(method, orderBy.Field);
            }

            expression = expression.Skip(state.Skip);
            expression = expression.Take(state.Take);

            return expression.ToLambda();
        }

        public static LambdaExpression ToPredicateExpression<TEntity>(this PageState state)
        {
            QueryExpression<TEntity> expression = new QueryExpression<TEntity>();

            if (state.Filter != null && state.Filter.Filters != null && state.Filter.Filters.Count() != 0)
            {
                ParameterExpression param = QueryExpressionHelpers.GetObjectParameter<TEntity>("q");

                var predicate = Expression.Lambda<Func<TEntity, bool>>(
                    GetPredicateExpression<TEntity>(state.Filter, param),
                    param);

                expression = expression.Where(predicate);
            }

            return expression.ToLambda();
        }

        private static Expression GetPredicateExpression<TEntity>(
            PageStateFilters filter, ParameterExpression param)
        {
            Expression predicate = null;

            if (filter != null && filter.Filters != null && filter.Filters.Count != 0)
            {
                var logic = filter.Logic;

                foreach (var entry in filter.Filters)
                {
                    var expression = GetPredicateExpression<TEntity>(entry, param);

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
                predicate = QueryExpressionHelpers.GetMethodExpression<TEntity>(
                    param, filter.Field, filter.Operator, filter.Value);
            }

            return predicate;
        }
    }
}
