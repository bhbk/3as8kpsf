using Bhbk.Lib.QueryExpression.Exceptions;
using Bhbk.Lib.QueryExpression.Extensions;
using Bhbk.Lib.QueryExpression.Factories;
using Bhbk.Lib.QueryExpression.Tests.Models;
using System;
using System.Linq;
using Xunit;

namespace Bhbk.Lib.QueryExpression.Tests.ExpressionTests
{
    public class QueryExpressionTests
    {
        [Fact]
        public void QueryExpr_Fail_OrderBy()
        {
            Assert.Throws<QueryExpressionPropertyException>(() => 
                QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().OrderBy("invalid").ToLambda());

            Assert.Throws<QueryExpressionSkipException>(() => 
                QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().OrderBy("int1").Skip(-1).Take(1000).ToLambda());

            Assert.Throws<QueryExpressionTakeException>(() => 
                QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().OrderBy("int1").Skip(1000).Take(0).ToLambda());

            Assert.Throws<QueryExpressionPropertyException>(() => 
                QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().OrderByDescending("invalid").ToLambda());

            Assert.Throws<QueryExpressionSkipException>(() => 
                QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().OrderByDescending("int1").Skip(-1).Take(1000).ToLambda());

            Assert.Throws<QueryExpressionTakeException>(() => 
                QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().OrderByDescending("int1").Skip(1000).Take(0).ToLambda());
        }

        [Fact]
        public void QueryExpr_Success_OrderBy()
        {
            var expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().OrderBy("guid1").ToLambda();
            expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().OrderBy("date1").Skip(1000).ToLambda();
            expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().OrderBy("int1").Skip(1000).Take(1000).ToLambda();
            expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().OrderByDescending("guid1").ToLambda();
            expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().OrderByDescending("date1").Skip(1000).ToLambda();
            expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().OrderByDescending("int1").Skip(1000).Take(1000).ToLambda();
        }

        [Fact]
        public void QueryExpr_Success_Where()
        {
            var expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().Where(x => x.guid1 == Guid.NewGuid()).ToLambda();
            expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().Where(x => x.date1 > DateTime.Now).ToLambda();
            expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().Where(x => x.int1 > 1000).ToLambda();
            expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().Where(x => x.string1.Contains("1000")).ToLambda();

            expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().Where(x => x.child1.guid1 == Guid.NewGuid()).ToLambda();
            expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().Where(x => x.child1.date1 == DateTime.Now).ToLambda();
            expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().Where(x => x.child1.int1 > 1000).ToLambda();
            expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().Where(x => x.child1.string1.Contains("1000")).ToLambda();

            expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().Where(x => x.child2.Any(y => y.guid1 == Guid.NewGuid())).ToLambda();
            expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().Where(x => x.child2.Any(y => y.date1 == DateTime.Now)).ToLambda();
            expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().Where(x => x.child2.Any(y => y.int1 > 1000)).ToLambda();
            expression = QueryExpressionFactory.GetQueryExpression<QueryExpressionModel>().Where(x => x.child2.Any(y => y.string1.Contains("1000"))).ToLambda();
        }
    }
}
