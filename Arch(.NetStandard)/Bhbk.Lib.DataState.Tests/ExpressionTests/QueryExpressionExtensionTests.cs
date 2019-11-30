using Bhbk.Lib.DataState.Expressions;
using Bhbk.Lib.DataState.Tests.Models;
using System;
using System.Linq;
using Xunit;

namespace Bhbk.Lib.DataState.Tests.ExpressionTests
{
    public class QueryExpressionExtensionTests
    {
        [Fact]
        public void Expr_QueryExpressionExtensions_Fail_OrderBy()
        {
            Assert.Throws<QueryExpressionPropertyException>(() =>
            {
                var expression = new QueryExpression<SampleEntity>().OrderBy("invalid").ToLambda();
            });

            Assert.Throws<QueryExpressionSkipException>(() =>
            {
                var expression = new QueryExpression<SampleEntity>().OrderBy("int1").Skip(-1).Take(1000).ToLambda();
            });

            Assert.Throws<QueryExpressionTakeException>(() =>
            {
                var expression = new QueryExpression<SampleEntity>().OrderBy("int1").Skip(1000).Take(0).ToLambda();
            });

            Assert.Throws<QueryExpressionPropertyException>(() =>
            {
                var expression = new QueryExpression<SampleEntity>().OrderByDescending("invalid").ToLambda();
            });

            Assert.Throws<QueryExpressionSkipException>(() =>
            {
                var expression = new QueryExpression<SampleEntity>().OrderByDescending("int1").Skip(-1).Take(1000).ToLambda();
            });

            Assert.Throws<QueryExpressionTakeException>(() =>
            {
                var expression = new QueryExpression<SampleEntity>().OrderByDescending("int1").Skip(1000).Take(0).ToLambda();
            });
        }

        [Fact(Skip = "NotImplemented")]
        public void Expr_QueryExpressionExtensions_Fail_Where()
        {

        }

        [Fact]
        public void Expr_QueryExpressionExtensions_Success_OrderBy()
        {
            var expression = new QueryExpression<SampleEntity>().OrderBy("guid1").ToLambda();
            expression = new QueryExpression<SampleEntity>().OrderBy("date1").Skip(1000).ToLambda();
            expression = new QueryExpression<SampleEntity>().OrderBy("int1").Skip(1000).Take(1000).ToLambda();
            expression = new QueryExpression<SampleEntity>().OrderByDescending("guid1").ToLambda();
            expression = new QueryExpression<SampleEntity>().OrderByDescending("date1").Skip(1000).ToLambda();
            expression = new QueryExpression<SampleEntity>().OrderByDescending("int1").Skip(1000).Take(1000).ToLambda();
        }

        [Fact]
        public void Expr_QueryExpressionExtensions_Success_Where()
        {
            var expression = new QueryExpression<SampleEntity>().Where(x => x.guid1 == Guid.NewGuid()).ToLambda();
            expression = new QueryExpression<SampleEntity>().Where(x => x.date1 > DateTime.Now).ToLambda();
            expression = new QueryExpression<SampleEntity>().Where(x => x.int1 > 1000).ToLambda();
            expression = new QueryExpression<SampleEntity>().Where(x => x.string1.Contains("1000")).ToLambda();

            expression = new QueryExpression<SampleEntity>().Where(x => x.child1.guid1 == Guid.NewGuid()).ToLambda();
            expression = new QueryExpression<SampleEntity>().Where(x => x.child1.date1 == DateTime.Now).ToLambda();
            expression = new QueryExpression<SampleEntity>().Where(x => x.child1.int1 > 1000).ToLambda();
            expression = new QueryExpression<SampleEntity>().Where(x => x.child1.string1.Contains("1000")).ToLambda();

            expression = new QueryExpression<SampleEntity>().Where(x => x.child2.Any(y => y.guid1 == Guid.NewGuid())).ToLambda();
            expression = new QueryExpression<SampleEntity>().Where(x => x.child2.Any(y => y.date1 == DateTime.Now)).ToLambda();
            expression = new QueryExpression<SampleEntity>().Where(x => x.child2.Any(y => y.int1 > 1000)).ToLambda();
            expression = new QueryExpression<SampleEntity>().Where(x => x.child2.Any(y => y.string1.Contains("1000"))).ToLambda();
        }
    }
}
