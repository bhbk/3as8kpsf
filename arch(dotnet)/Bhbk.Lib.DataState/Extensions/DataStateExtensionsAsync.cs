using Bhbk.Lib.DataState.Interfaces;
using Bhbk.Lib.QueryExpression;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bhbk.Lib.DataState.Extensions
{
    public static class DataStateExtensionsAsync
    {
        public async static Task<LambdaExpression> ApplyStateAsync<TEntity>(
            this Task<IQueryExpression<TEntity>> query, IDataState state)
            => await Task.FromResult((await query).ApplyState(state));

        public async static Task<LambdaExpression> ApplyPredicateAsync<TEntity>(
            this Task<IQueryExpression<TEntity>> query, IDataState state)
            => await Task.FromResult((await query).ApplyPredicate(state));

        public async static Task<bool> SetDefaultSortAsync(
            this IDataState state, string field, string direction = "asc")
            => await Task.FromResult(state.SetDefaultSort(field, direction));

        public async static Task<bool> SetDefaultSortAsync(
            this IDataState state, ICollection<IDataStateSort> defaultSortDescriptors)
            => await Task.FromResult(state.SetDefaultSort(defaultSortDescriptors));
    }
}
