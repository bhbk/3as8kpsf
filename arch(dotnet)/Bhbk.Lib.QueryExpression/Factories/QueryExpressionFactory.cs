using Bhbk.Lib.QueryExpression.Extensions;
using System.Threading.Tasks;

namespace Bhbk.Lib.QueryExpression.Factories
{
    public static class QueryExpressionFactory
    {
        public static IQueryExpression<TEntity> GetQueryExpression<TEntity>() =>
            new QueryExpression<TEntity>();

        public async static Task<IQueryExpression<TEntity>> GetQueryExpressionAsync<TEntity>() =>
            await Task.FromResult(new QueryExpression<TEntity>());
    }
}
