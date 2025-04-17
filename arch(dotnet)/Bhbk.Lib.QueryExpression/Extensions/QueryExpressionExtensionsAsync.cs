using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bhbk.Lib.QueryExpression.Extensions
{
    public static class QueryExpressionExtensionsAsync
    {
        public async static Task<IQueryExpression<TEntity>> OrderByAsync<TEntity>(
            this Task<IQueryExpression<TEntity>> query, string field)
            => await query.OrderByAsync("OrderBy", field);

        private async static Task<IQueryExpression<TEntity>> OrderByAsync<TEntity>(
            this Task<IQueryExpression<TEntity>> query, string method, string field)
            => await Task.FromResult((await query).OrderBy(method, field));

        public async static Task<IQueryExpression<TEntity>> OrderByDescendingAsync<TEntity>(
            this Task<IQueryExpression<TEntity>> query, string field)
            => await query.OrderByAsync("OrderByDescending", field);

        public async static Task<IQueryExpression<TEntity>> SkipAsync<TEntity>(
            this Task<IQueryExpression<TEntity>> query, int skip)
            => await Task.FromResult((await query).Skip(skip));

        public async static Task<IQueryExpression<TEntity>> SkipAsync<TEntity>(
            this Task<IQueryExpression<TEntity>> query, int skip, int take)
            => await Task.FromResult((await query).Skip(skip, take));

        public async static Task<IQueryExpression<TEntity>> TakeAsync<TEntity>(
            this Task<IQueryExpression<TEntity>> query, int take)
            => await Task.FromResult((await query).Take(take));

        public async static Task<IQueryExpression<TEntity>> ThenByAsync<TEntity>(
            this Task<IQueryExpression<TEntity>> query, string field)
            => await query.OrderByAsync("ThenBy", field);

        public async static Task<IQueryExpression<TEntity>> ThenByDescendingAsync<TEntity>(
            this Task<IQueryExpression<TEntity>> query, string field)
            => await query.OrderByAsync("ThenByDescending", field);

        public async static Task<IQueryExpression<TEntity>> WhereAsync<TEntity>(
            this Task<IQueryExpression<TEntity>> query, Expression<Func<TEntity, bool>> expression)
            => await Task.FromResult((await query).Where(expression));

        public async static Task<LambdaExpression> ToLambdaAsync<TEntity>(
            this Task<IQueryExpression<TEntity>> query)
            => query.IsFaulted
                ? throw query.Exception.GetBaseException()
                : await Task.FromResult((await query).ToLambda());
    }
}
