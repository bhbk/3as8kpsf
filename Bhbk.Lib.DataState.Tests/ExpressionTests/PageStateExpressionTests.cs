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
                    Filter = new RecursiveFilterModel()
                    {
                        Logic = "and",
                        Filters = new List<RecursiveFilterModel>()
                        {
                            new RecursiveFilterModel { Field = AlphaNumeric.CreateString(8), Operator = "contains", Value = "1000" },
                            new RecursiveFilterModel {
                                Logic = "or",
                                Filters = new List<RecursiveFilterModel>()
                                {
                                    new RecursiveFilterModel { Field = "int1", Operator = "eq", Value = "1000 "},
                                }
                            }
                        }
                    }
                };

                var predicate = state.ToPredicateExpression<TestModel>();
            });
        }

        [Fact]
        public void Expr_PageState_Fail_Fields_Sort()
        {
            Assert.Throws<QueryExpressionPropertyException>(() =>
            {
                var state = new PageState()
                {
                    Sort = new List<SortModel>()
                    {
                        new SortModel() { Field = AlphaNumeric.CreateString(8), Dir = "asc" },
                    },
                    Skip = 0,
                    Take = 1000
                };

                var expression = state.ToExpression<TestModel>();
            });

            Assert.Throws<QueryExpressionSortException>(() =>
            {
                var state = new PageState()
                {
                    Sort = new List<SortModel>()
                    {
                        new SortModel() { Field = "string1", Dir = AlphaNumeric.CreateString(8) },
                    },
                    Skip = 0,
                    Take = 1000
                };

                var expression = state.ToExpression<TestModel>();
            });
        }

        [Fact]
        public void Expr_PageState_Fail_Skip()
        {
            Assert.Throws<QueryExpressionSkipException>(() =>
            {
                var state = new PageState()
                {
                    Sort = new List<SortModel>()
                    {
                        new SortModel() { Field = "string1", Dir = "asc" },
                    },
                    Skip = -1000,
                    Take = 1000
                };

                var expression = state.ToExpression<TestModel>();
            });
        }

        [Fact]
        public void Expr_PageState_Fail_Take()
        {
            Assert.Throws<QueryExpressionTakeException>(() =>
            {
                var state = new PageState()
                {
                    Sort = new List<SortModel>()
                    {
                        new SortModel() { Field = "string1", Dir = "asc" },
                    },
                    Skip = 0,
                    Take = 0
                };

                var expression = state.ToExpression<TestModel>();
            });
        }

        [Fact]
        public void Expr_PageState_Success()
        {
            var state = new PageState()
            {
                Filter = new RecursiveFilterModel()
                {
                    Logic = "and",
                    Filters = new List<RecursiveFilterModel>()
                    {
                        new RecursiveFilterModel { Field = "string1", Operator = "contains", Value = "x" },
                        new RecursiveFilterModel { Field = "string1", Operator = "startswith", Value = "y" },
                        new RecursiveFilterModel {
                            Logic = "or",
                            Filters = new List<RecursiveFilterModel>()
                            {
                                new RecursiveFilterModel { Field = "int1", Operator = "eq", Value = "1000 "},
                                new RecursiveFilterModel { Field = "int1", Operator = "gt", Value = "1000 "},
                            }
                        }
                    }
                },
                Sort = new List<SortModel>()
                {
                    new SortModel() { Field = "guid1", Dir = "asc" },
                    new SortModel() { Field = "guid2", Dir = "asc" },
                    new SortModel() { Field = "date1", Dir = "asc" },
                    new SortModel() { Field = "date2", Dir = "asc" },
                    new SortModel() { Field = "int1", Dir = "asc" },
                    new SortModel() { Field = "int2", Dir = "asc" },
                    new SortModel() { Field = "decimal1", Dir = "asc" },
                    new SortModel() { Field = "decimal2", Dir = "asc" },
                    new SortModel() { Field = "bool1", Dir = "asc" },
                    new SortModel() { Field = "bool2", Dir = "asc" },
                    new SortModel() { Field = "string1", Dir = "asc" },
                },
                Skip = 0,
                Take = 1000
            };

            var expression = state.ToExpression<TestModel>();
            var predicate = state.ToPredicateExpression<TestModel>();
        }
    }
}
