using Bhbk.Lib.QueryExpression.Exceptions;
using Bhbk.Lib.QueryExpression.Extensions;
using Bhbk.Lib.QueryExpression.Factories;
using Bhbk.Lib.QueryExpression.Tests.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Bhbk.Lib.QueryExpression.Tests.ExpressionTests
{
    public class QueryExpressionTests
    {
        [Theory]
        [InlineData("string1", "contains", "value")]
        [InlineData("string1", "doesnotcontain", "value")]
        [InlineData("string1", "startswith", "value")]
        [InlineData("string1", "doesnotstartswith", "value")]
        [InlineData("string1", "endswith", "value")]
        [InlineData("string1", "doesnotendwith", "value")]
        [InlineData("string1", "eq", "value")]
        [InlineData("string1", "neq", "value")]
        [InlineData("string1", "isnullorempty", null)]
        [InlineData("string1", "isnotnullorempty", null)]
        [InlineData("int1", "eq", "100")]
        [InlineData("int1", "neq", "100")]
        [InlineData("int1", "gt", "100")]
        [InlineData("int1", "gte", "100")]
        [InlineData("int1", "lt", "100")]
        [InlineData("int1", "lte", "100")]
        public void QueryExpr_Success_Operator(string field, string op, string value)
        {
            ExpressionFactory.GetMethodExpression<QueryExpressionModel>(
                ExpressionFactory.GetObjectParameter<QueryExpressionModel>("x"), field, op, value);
        }

        [Theory]
        [InlineData("date1", "1-1-20")]
        [InlineData("date2", "1-1-20")]
        [InlineData("date2", null)]
        [InlineData("guid1", "0bb2871b-0b3a-468c-baf1-c24ed66a7de1")]
        [InlineData("guid2", "0bb2871b-0b3a-468c-baf1-c24ed66a7de1")]
        [InlineData("guid2", null)]
        [InlineData("int1", "10")]
        [InlineData("int2", "10")]
        [InlineData("int2", null)]
        [InlineData("decimal1", "10.5")]
        [InlineData("decimal2", "10.5")]
        [InlineData("decimal2", null)]
        [InlineData("bool1", "true")]
        [InlineData("bool2", "true")]
        [InlineData("bool2", null)]
        [InlineData("string1", "test")]
        [InlineData("string1", null)]
        public void QueryExpr_Success_Constant(string field, string value)
        {
            Assert.IsAssignableFrom<ConstantExpression>(ExpressionFactory.GetConstantExpression<QueryExpressionModel>(field, value));
        }

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
