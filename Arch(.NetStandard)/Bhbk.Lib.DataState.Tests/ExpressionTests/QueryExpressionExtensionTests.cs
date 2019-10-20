using Bhbk.Lib.DataState.Expressions;
using Bhbk.Lib.DataState.Tests.Models;
using System.Linq;
using Xunit;
using System;

namespace Bhbk.Lib.DataState.Tests.ExpressionTests
{
    public class QueryExpressionExtensionTests
    {
        [Fact]
        public void Expr_QueryExpressionExtensions_Fail_OrderBy()
        {
            Assert.Throws<QueryExpressionPropertyException>(() =>
            {
                var orderBy = new QueryExpression<SampleEntity>().OrderBy("invalid").ToLambda();
                orderBy = new QueryExpression<SampleEntity>().OrderByDescending("invalid").ToLambda();
            });

            Assert.Throws<QueryExpressionSkipException>(() =>
            {
                var skip = new QueryExpression<SampleEntity>().OrderBy("int1").Skip(-1000).Take(1000).ToLambda();
                skip = new QueryExpression<SampleEntity>().OrderByDescending("int1").Skip(-1000).Take(1000).ToLambda();
            });

            Assert.Throws<QueryExpressionTakeException>(() =>
            {
                var take = new QueryExpression<SampleEntity>().OrderBy("int1").Skip(1000).Take(0).ToLambda();
                take = new QueryExpression<SampleEntity>().OrderByDescending("int1").Skip(1000).Take(0).ToLambda();
            });
        }

        [Fact(Skip = "NotImplemented")]
        public void Expr_QueryExpressionExtensions_Fail_Where()
        {

        }

        [Fact]
        public void Expr_QueryExpressionExtensions_Success()
        {
            var orderBy = new QueryExpression<SampleEntity>().OrderBy("guid1").ToLambda();
            orderBy = new QueryExpression<SampleEntity>().OrderBy("date1").Skip(1000).ToLambda();
            orderBy = new QueryExpression<SampleEntity>().OrderBy("int1").Skip(1000).Take(1000).ToLambda();
            orderBy = new QueryExpression<SampleEntity>().OrderByDescending("guid1").ToLambda();
            orderBy = new QueryExpression<SampleEntity>().OrderByDescending("date1").Skip(1000).ToLambda();
            orderBy = new QueryExpression<SampleEntity>().OrderByDescending("int1").Skip(1000).Take(1000).ToLambda();

            var where = new QueryExpression<SampleEntity>().Where(x => x.guid1 == Guid.NewGuid()).ToLambda();
            where = new QueryExpression<SampleEntity>().Where(x => x.date1 > DateTime.Now).ToLambda();
            where = new QueryExpression<SampleEntity>().Where(x => x.int1 > 1000).ToLambda();
            where = new QueryExpression<SampleEntity>().Where(x => x.string1.Contains("1000")).ToLambda();
        }
    }
}
