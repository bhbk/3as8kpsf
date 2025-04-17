using Bhbk.Lib.DataState.Interfaces;
using Bhbk.Lib.DataState.Models;
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
    public static class DataStateExtensions
    {
        /*
         * Sets default sort if no valid sort exists. Returns true if default was applied.
         */
        public static bool SetDefaultSort(this IDataState state, string field, string direction = "asc")
        {
            if (!PagerStateSort.TryParse(field, direction, out ICollection<IDataStateSort> defaultSort))
                throw new ArgumentException("Default values cannot be null or contain whitespace");

            if (!PagerStateSort.TryParse(state.Sort, out ICollection<IDataStateSort> sortDescriptors))
                throw new ArgumentException("Sort descriptor properties cannot be null or contain whitespace");

            var result = sortDescriptors.Count == 0;

            state.Sort = result ? defaultSort : sortDescriptors;

            return result;
        }

        public static bool SetDefaultSort(this IDataState state, ICollection<IDataStateSort> defaultSortDescriptors)
        {
            if (!PagerStateSort.TryParse(defaultSortDescriptors, out ICollection<IDataStateSort> defaultSort))
                throw new ArgumentException("Default values cannot be null or contain whitespace");

            if (!PagerStateSort.TryParse(state.Sort, out ICollection<IDataStateSort> sortDescriptors))
                throw new ArgumentException("Sort descriptor properties cannot be null or contain whitespace");

            var result = sortDescriptors.Count == 0;

            state.Sort = result ? defaultSort : sortDescriptors;

            return result;
        }

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
            if (filter == null)
                return false;

            // Check for nested filters (composite filter)
            if (filter.Filters != null && filter.Filters.Count() != 0)
                return true;

            // Check for simple filter (Field + Operator)
            if (!string.IsNullOrEmpty(filter.Field) && !string.IsNullOrEmpty(filter.Operator))
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

        /// <summary>
        /// Applies paging (sort, skip, take) to IQueryExpression from IDataState object.
        /// </summary>
        public static IQueryExpression<TEntity> ApplyPaging<TEntity>(
            this IQueryExpression<TEntity> query, IDataState state)
            => query.ApplySort(state).Skip(state.Skip).Take(state.Take);

        /// <summary>
        /// Applies sorting to IQueryExpression from IDataState object.
        /// </summary>
        public static IQueryExpression<TEntity> ApplySort<TEntity>(
            this IQueryExpression<TEntity> query, IDataState state)
        {
            if (!IsSortValid(state.Sort))
                throw new QueryExpressionSortException($"The value for sort is invalid.");

            string method = string.Empty;

            foreach (var orderBy in state.Sort.Where(x => x.Dir != null))
            {
                if (method == string.Empty)
                    method = orderBy.Dir == "asc" ? "OrderBy" : "OrderByDescending";
                else
                    method = orderBy.Dir == "asc" ? "ThenBy" : "ThenByDescending";

                query = query.OrderBy(method, orderBy.Field);
            }

            return query;
        }

        /// <summary>
        /// Changes value of filter field to new value.
        /// </summary>
        public static IDataStateFilter SetFilterFieldValue(
            this IDataState state, IDataStateFilter filter, string originalFieldValue, string updatedFieldValue)
        {
            if (filter.Filters != null
                && filter.Filters.Count() > 0)
            {
                for (int i = 0; i < filter.Filters.Count(); i++)
                {
                    if (filter.Filters.ElementAt(i).Field == originalFieldValue)
                        filter.Filters.ElementAt(i).Field = updatedFieldValue;

                    if (filter.Filters.ElementAt(i).Filters != null)
                        state.SetFilterFieldValue(filter.Filters.ElementAt(i), originalFieldValue, updatedFieldValue);
                }
            }

            return filter;
        }

        /// <summary>
        /// Changes value of sort field to new value.
        /// </summary>
        public static ICollection<IDataStateSort> SetSortFieldValue(
            this IDataState state, ICollection<IDataStateSort> sort, string originalFieldValue, string updatedFieldValue)
        {
            if (sort.Count() > 0)
            {
                for (int i = 0; i < sort.Count(); i++)
                {
                    if (sort.ElementAt(i).Field == originalFieldValue)
                        sort.ElementAt(i).Field = updatedFieldValue;
                }
            }

            return sort;
        }
    }
}
