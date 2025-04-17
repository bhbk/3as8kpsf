using Bhbk.Lib.QueryExpression.Exceptions;
using Bhbk.Lib.QueryExpression.Extensions;
using Bhbk.Lib.QueryExpression.Factories;
using Bhbk.Lib.QueryExpression.Tests.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Bhbk.Lib.QueryExpression.Tests.ExpressionTests
{
    public class QueryExpressionAsyncTests
    {
        [Fact]
        public async Task QueryExpr_Fail_OrderByAsync()
        {
            await Assert.ThrowsAsync<QueryExpressionPropertyException>(() =>
                QueryExpressionFactory.GetQueryExpressionAsync<QueryExpressionModel>().OrderByAsync("invalid").ToLambdaAsync());

            await Assert.ThrowsAsync<QueryExpressionPropertyException>(() =>
                QueryExpressionFactory.GetQueryExpressionAsync<QueryExpressionModel>().OrderByDescendingAsync("invalid").ToLambdaAsync());

            await Assert.ThrowsAsync<QueryExpressionTakeException>(() =>
                QueryExpressionFactory.GetQueryExpressionAsync<QueryExpressionModel>().OrderByAsync("int1").SkipAsync(1000).TakeAsync(0).ToLambdaAsync());

            await Assert.ThrowsAsync<QueryExpressionSkipException>(() =>
                QueryExpressionFactory.GetQueryExpressionAsync<QueryExpressionModel>().OrderByDescendingAsync("int1").SkipAsync(-1).TakeAsync(1000).ToLambdaAsync());

            await Assert.ThrowsAsync<QueryExpressionTakeException>(() =>
                QueryExpressionFactory.GetQueryExpressionAsync<QueryExpressionModel>().OrderByDescendingAsync("int1").SkipAsync(1000).TakeAsync(0).ToLambdaAsync());
        }

        [Fact]
        public async Task QueryExpr_Success_OrderByAsync()
        {
            var tasks = new List<Task>()
            {
                QueryExpressionFactory.GetQueryExpressionAsync<QueryExpressionModel>().OrderByAsync("int1").ToLambdaAsync(),
                QueryExpressionFactory.GetQueryExpressionAsync<QueryExpressionModel>().OrderByAsync("guid1").SkipAsync(1).ToLambdaAsync(),
                QueryExpressionFactory.GetQueryExpressionAsync<QueryExpressionModel>().OrderByAsync("string1").SkipAsync(1).TakeAsync(1000).ToLambdaAsync(),
                QueryExpressionFactory.GetQueryExpressionAsync<QueryExpressionModel>().OrderByDescendingAsync("int1").ToLambdaAsync(),
                QueryExpressionFactory.GetQueryExpressionAsync<QueryExpressionModel>().OrderByDescendingAsync("guid1").SkipAsync(1).ToLambdaAsync(),
                QueryExpressionFactory.GetQueryExpressionAsync<QueryExpressionModel>().OrderByDescendingAsync("string1").SkipAsync(1).TakeAsync(1000).ToLambdaAsync()
            };

            await Task.WhenAll(tasks);
        }

        [Fact]
        public async Task QueryExpr_Success_WhereAsync()
        {
            await Task.WhenAll(new List<Task>
            {
                QueryExpressionFactory.GetQueryExpressionAsync<QueryExpressionModel>().WhereAsync(x => x.int1 > 1000).ToLambdaAsync(),
                QueryExpressionFactory.GetQueryExpressionAsync<QueryExpressionModel>().WhereAsync(x => x.string1.Contains("1000")).ToLambdaAsync()
            });
        }
    }
}
