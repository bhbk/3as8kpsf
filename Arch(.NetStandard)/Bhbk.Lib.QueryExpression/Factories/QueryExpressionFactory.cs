using Bhbk.Lib.QueryExpression.Extensions;
using System.Threading.Tasks;

namespace Bhbk.Lib.QueryExpression.Factories
{
    public static class QueryExpressionFactory
    {
        public static QueryExpression<TEntity> GetQueryExpression<TEntity>() =>
            new QueryExpression<TEntity>();

        public async static Task<QueryExpression<TEntity>> GetQueryExpressionAsync<TEntity>() =>
            await Task.FromResult(new QueryExpression<TEntity>());
    }
}
