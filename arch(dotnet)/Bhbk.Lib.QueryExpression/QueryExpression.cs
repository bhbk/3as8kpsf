using Bhbk.Lib.QueryExpression.Factories;
using System.Linq.Expressions;

namespace Bhbk.Lib.QueryExpression
{
    public class QueryExpression<TEntity> : IQueryExpression<TEntity>
    {
        public Expression Body { get; set; }
        public ParameterExpression Param { get; set; } = ExpressionFactory.GetQueryParameter<TEntity>();
    }
}
