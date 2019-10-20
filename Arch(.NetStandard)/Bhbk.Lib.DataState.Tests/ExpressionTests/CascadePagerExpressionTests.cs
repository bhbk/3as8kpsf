using Bhbk.Lib.Cryptography.Entropy;
using Bhbk.Lib.DataState.Expressions;
using Bhbk.Lib.DataState.Models;
using Bhbk.Lib.DataState.Tests.Models;
using System.Collections.Generic;
using Xunit;

namespace Bhbk.Lib.DataState.Tests.ExpressionTests
{
    public class CascadePagerExpressionTests
    {
        [Fact]
        public void Expr_CascadePager_Fail_Sort()
        {
            Assert.Throws<QueryExpressionPropertyException>(() =>
            {
                var state = new CascadePager()
                {
                    Sort = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>(AlphaNumeric.CreateString(8), "asc"),
                    },
                    Skip = 0,
                    Take = 1000
                };

                var expression = state.ToExpression<SampleEntity>();
            });

            Assert.Throws<QueryExpressionSortException>(() =>
            {
                var state = new CascadePager()
                {
                    Sort = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("string1", AlphaNumeric.CreateString(8)),
                    },
                    Skip = 0,
                    Take = 1000
                };

                var expression = state.ToExpression<SampleEntity>();
            });
        }

        [Fact]
        public void Expr_CascadePager_Fail_Sort_Skip()
        {
            Assert.Throws<QueryExpressionSkipException>(() =>
            {
                var state = new CascadePager()
                {
                    Sort = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("string1", "asc"),
                    },
                    Skip = -1,
                    Take = 1000
                };

                var expression = state.ToExpression<SampleEntity>();
            });
        }

        [Fact]
        public void Expr_CascadePager_Fail_Sort_Take()
        {
            Assert.Throws<QueryExpressionTakeException>(() =>
            {
                var state = new CascadePager()
                {
                    Sort = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("string1", "asc"),
                    },
                    Skip = 0,
                    Take = 0
                };

                var expression = state.ToExpression<SampleEntity>();
            });
        }

        [Fact]
        public void Expr_CascadePager_Success_Sort()
        {
            var state = new CascadePager()
            {
                Sort = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("guid1", "asc"),
                    new KeyValuePair<string, string>("guid2", "desc"),
                    new KeyValuePair<string, string>("date1", "asc"),
                    new KeyValuePair<string, string>("date2", "desc"),
                    new KeyValuePair<string, string>("int1", "asc"),
                    new KeyValuePair<string, string>("int2", "desc"),
                    new KeyValuePair<string, string>("decimal1", "asc"),
                    new KeyValuePair<string, string>("decimal2", "desc"),
                    new KeyValuePair<string, string>("bool1", "asc"),
                    new KeyValuePair<string, string>("bool2", "desc"),
                    new KeyValuePair<string, string>("string1", "asc"),
                },
                Skip = 0,
                Take = 1000
            };

            var predicate = state.ToPredicateExpression<SampleEntity>();
            var expression = state.ToExpression<SampleEntity>();
        }
    }
}
