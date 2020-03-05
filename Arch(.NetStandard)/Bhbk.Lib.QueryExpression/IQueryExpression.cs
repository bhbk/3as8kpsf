using System.Linq.Expressions;

namespace Bhbk.Lib.QueryExpression
{
    public interface IQueryExpression<TEntity>
    {
        Expression Body { get; set; }
        ParameterExpression Param { get; set; }
    }
}
