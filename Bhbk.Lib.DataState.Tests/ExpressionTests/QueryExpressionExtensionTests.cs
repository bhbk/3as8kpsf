using Bhbk.Lib.DataState.Expressions;
using Bhbk.Lib.DataState.Tests.Models;
using System.Linq;
using Xunit;

namespace Bhbk.Lib.DataState.Tests.ExpressionTests
{
    public class QueryExpressionExtensionTests
    {
        [Fact(Skip = "NotImplemented")]
        public void Expr_QueryExpressionExtensions_Fail()
        {
            Assert.Throws<QueryExpressionTakeException>(() =>
            {

            });
        }

        [Fact]
        public void Expr_QueryExpressionExtensions_Success()
        {
            var first = new QueryExpression<TestModel>().First().ToLambda();
            first = new QueryExpression<TestModel>().First(x => x.int1 > 1000).ToLambda();

            var last = new QueryExpression<TestModel>().Last().ToLambda();
            last = new QueryExpression<TestModel>().Last(x => x.int1 > 1000).ToLambda();

            var where = new QueryExpression<TestModel>().Where(x => x.int1 > 1000).ToLambda();

            var methods = typeof(Queryable).GetMethods()
                .Where(x => x.Name.Contains("OrderBy")).ToArray();
        }
    }
}
