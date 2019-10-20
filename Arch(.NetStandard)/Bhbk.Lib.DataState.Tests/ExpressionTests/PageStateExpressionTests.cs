using Bhbk.Lib.Cryptography.Entropy;
using Bhbk.Lib.DataState.Expressions;
using Bhbk.Lib.DataState.Models;
using Bhbk.Lib.DataState.Tests.Models;
using System.Collections.Generic;
using Xunit;
using static Bhbk.Lib.DataState.Models.PageState;

namespace Bhbk.Lib.DataState.Tests.ExpressionTests
{
    public class PageStateExpressionTests
    {
        [Fact]
        public void Expr_PageState_Fail_Fields_Filter()
        {
            Assert.Throws<QueryExpressionPropertyException>(() =>
            {
                var state = new PageState()
                {
                    Filter = new PageStateFilters()
                    {
                        Logic = "and",
                        Filters = new List<PageStateFilters>()
                        {
                            new PageStateFilters { Field = AlphaNumeric.CreateString(8), Operator = "contains", Value = "1000" },
                            new PageStateFilters {
                                Logic = "or",
                                Filters = new List<PageStateFilters>()
                                {
                                    new PageStateFilters { Field = "int1", Operator = "eq", Value = "1000 "},
                                }
                            }
                        }
                    }
                };

                var predicate = state.ToPredicateExpression<SampleEntity>();
            });
        }

        [Fact]
        public void Expr_PageState_Fail_Fields_Sort()
        {
            Assert.Throws<QueryExpressionPropertyException>(() =>
            {
                var state = new PageState()
                {
                    Sort = new List<PageStateSort>()
                    {
                        new PageStateSort() { Field = AlphaNumeric.CreateString(8), Dir = "asc" },
                    },
                    Skip = 0,
                    Take = 1000
                };

                var expression = state.ToExpression<SampleEntity>();
            });

            Assert.Throws<QueryExpressionSortException>(() =>
            {
                var state = new PageState()
                {
                    Sort = new List<PageStateSort>()
                    {
                        new PageStateSort() { Field = "string1", Dir = AlphaNumeric.CreateString(8) },
                    },
                    Skip = 0,
                    Take = 1000
                };

                var expression = state.ToExpression<SampleEntity>();
            });
        }

        [Fact]
        public void Expr_PageState_Fail_Skip()
        {
            Assert.Throws<QueryExpressionSkipException>(() =>
            {
                var state = new PageState()
                {
                    Sort = new List<PageStateSort>()
                    {
                        new PageStateSort() { Field = "string1", Dir = "asc" },
                    },
                    Skip = -1000,
                    Take = 1000
                };

                var expression = state.ToExpression<SampleEntity>();
            });
        }

        [Fact]
        public void Expr_PageState_Fail_Take()
        {
            Assert.Throws<QueryExpressionTakeException>(() =>
            {
                var state = new PageState()
                {
                    Sort = new List<PageStateSort>()
                    {
                        new PageStateSort() { Field = "string1", Dir = "asc" },
                    },
                    Skip = 0,
                    Take = 0
                };

                var expression = state.ToExpression<SampleEntity>();
            });
        }

        [Fact]
        public void Expr_PageState_Success()
        {
            var state = new PageState()
            {
                Filter = new PageStateFilters()
                {
                    Logic = "and",
                    Filters = new List<PageStateFilters>()
                    {
                        new PageStateFilters { Field = "string1", Operator = "contains", Value = "x" },
                        new PageStateFilters { Field = "string1", Operator = "startswith", Value = "y" },
                        new PageStateFilters {
                            Logic = "or",
                            Filters = new List<PageStateFilters>()
                            {
                                new PageStateFilters { Field = "int1", Operator = "eq", Value = "1000 "},
                                new PageStateFilters { Field = "int1", Operator = "gt", Value = "1000 "},
                            }
                        }
                    }
                },
                Sort = new List<PageStateSort>()
                {
                    new PageStateSort() { Field = "guid1", Dir = "asc" },
                    new PageStateSort() { Field = "guid2", Dir = "desc" },
                    new PageStateSort() { Field = "date1", Dir = "asc" },
                    new PageStateSort() { Field = "date2", Dir = "desc" },
                    new PageStateSort() { Field = "int1", Dir = "asc" },
                    new PageStateSort() { Field = "int2", Dir = "desc" },
                    new PageStateSort() { Field = "decimal1", Dir = "asc" },
                    new PageStateSort() { Field = "decimal2", Dir = "desc" },
                    new PageStateSort() { Field = "bool1", Dir = "asc" },
                    new PageStateSort() { Field = "bool2", Dir = "desc" },
                    new PageStateSort() { Field = "string1", Dir = "asc" },
                    new PageStateSort() { Field = "string1", Dir = "desc" },
                    new PageStateSort() { Field = "string1" },
                },
                Skip = 0,
                Take = 1000
            };

            var expression = state.ToExpression<SampleEntity>();
            var predicate = state.ToPredicateExpression<SampleEntity>();
        }
    }
}
