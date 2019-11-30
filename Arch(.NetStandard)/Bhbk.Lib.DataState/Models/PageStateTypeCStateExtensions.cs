﻿using Bhbk.Lib.DataState.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static Bhbk.Lib.DataState.Models.PageStateTypeC;

namespace Bhbk.Lib.DataState.Expressions
{
    public static class PageStateTypeCStateExtensions
    {
        public static LambdaExpression ToExpression<TEntity>(
            this PageStateTypeC state)
        {
            var expression = new QueryExpression<TEntity>();

            if (!IsSortValid(state.Sort))
                throw new QueryExpressionSortException($"The value for sort is invalid.");

            if (state.Skip < 0)
                throw new QueryExpressionSkipException(state.Skip);

            if (state.Take < 1)
                throw new QueryExpressionTakeException(state.Take);

            if (IsFilterValid(state.Filter))
            {
                var parameter = QueryExpressionHelpers.GetObjectParameter<TEntity>("q");

                var predicate = Expression.Lambda<Func<TEntity, bool>>(
                    GetPredicateExpression<TEntity>(state.Filter, parameter),
                    parameter);

                expression = expression.Where(predicate);
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

                expression = expression.OrderBy(method, orderBy.Field);
            }

            expression = expression.Skip(state.Skip);
            expression = expression.Take(state.Take);

            return expression.ToLambda();
        }

        public static LambdaExpression ToPredicateExpression<TEntity>(
            this PageStateTypeC state)
        {
            var expression = new QueryExpression<TEntity>();

            if (IsFilterValid(state.Filter))
            {
                var parameter = QueryExpressionHelpers.GetObjectParameter<TEntity>("q");

                var predicate = Expression.Lambda<Func<TEntity, bool>>(
                    GetPredicateExpression<TEntity>(state.Filter, parameter),
                    parameter);

                expression = expression.Where(predicate);
            }

            return expression.ToLambda();
        }

        private static Expression GetPredicateExpression<TEntity>(
            PageStateTypeCFilters filter, 
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
                predicate = QueryExpressionHelpers.GetMethodExpression<TEntity>(
                    parameter, filter.Field, filter.Operator, filter.Value);
            }

            return predicate;
        }

        internal static bool IsFilterValid(PageStateTypeCFilters filter)
        {
            if (filter != null
                && filter.Filters != null
                && filter.Filters.Count() != 0)
                return true;

            return false;
        }

        internal static bool IsSortValid(List<PageStateTypeCSort> sort)
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