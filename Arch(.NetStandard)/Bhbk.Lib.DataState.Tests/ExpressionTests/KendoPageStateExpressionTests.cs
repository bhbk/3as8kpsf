using Bhbk.Lib.Cryptography.Entropy;
using Bhbk.Lib.DataState.Expressions;
using Bhbk.Lib.DataState.Models;
using Bhbk.Lib.DataState.Tests.Models;
using System.Collections.Generic;
using Xunit;
using static Bhbk.Lib.DataState.Models.KendoPageState;

namespace Bhbk.Lib.DataState.Tests.ExpressionTests
{
    public class KendoPageStateExpressionTests
    {
        [Fact]
        public void Expr_KendoPageState_Fail_Filter()
        {
            Assert.Throws<QueryExpressionPropertyException>(() =>
            {
                var state = new KendoPageState()
                {
                    Filter = new KendoPageStateFilters()
                    {
                        Logic = "and",
                        Filters = new List<KendoPageStateFilters>()
                        {
                            new KendoPageStateFilters { Field = AlphaNumeric.CreateString(8), Operator = "contains", Value = "1000" },
                            new KendoPageStateFilters {
                                Logic = "or",
                                Filters = new List<KendoPageStateFilters>()
                                {
                                    new KendoPageStateFilters { Field = "int1", Operator = "eq", Value = "1000 "},
                                }
                            }
                        }
                    }
                };

                var predicate = state.ToPredicateExpression<SampleEntity>();
            });
        }

        [Fact]
        public void Expr_KendoPageState_Fail_Sort()
        {
            Assert.Throws<QueryExpressionPropertyException>(() =>
            {
                var state = new KendoPageState()
                {
                    Sort = new List<KendoPageStateSort>()
                    {
                        new KendoPageStateSort() { Field = AlphaNumeric.CreateString(8), Dir = "asc" },
                    },
                    Skip = 0,
                    Take = 1000
                };

                var expression = state.ToExpression<SampleEntity>();
            });

            Assert.Throws<QueryExpressionSortException>(() =>
            {
                var state = new KendoPageState()
                {
                    Sort = new List<KendoPageStateSort>()
                    {
                        new KendoPageStateSort() { Field = "string1", Dir = AlphaNumeric.CreateString(8) },
                    },
                    Skip = 0,
                    Take = 1000
                };

                var expression = state.ToExpression<SampleEntity>();
            });
        }

        [Fact]
        public void Expr_KendoPageState_Fail_Sort_Skip()
        {
            Assert.Throws<QueryExpressionSkipException>(() =>
            {
                var state = new KendoPageState()
                {
                    Sort = new List<KendoPageStateSort>()
                    {
                        new KendoPageStateSort() { Field = "string1", Dir = "asc" },
                    },
                    Skip = -1,
                    Take = 1000
                };

                var expression = state.ToExpression<SampleEntity>();
            });
        }

        [Fact]
        public void Expr_KendoPageState_Fail_Sort_Take()
        {
            Assert.Throws<QueryExpressionTakeException>(() =>
            {
                var state = new KendoPageState()
                {
                    Sort = new List<KendoPageStateSort>()
                    {
                        new KendoPageStateSort() { Field = "string1", Dir = "asc" },
                    },
                    Skip = 0,
                    Take = 0
                };

                var expression = state.ToExpression<SampleEntity>();
            });
        }

        [Fact]
        public void Expr_KendoPageState_Success_Filter()
        {
            var state = new KendoPageState()
            {
                Filter = new KendoPageStateFilters()
                {
                    Logic = "and",
                    Filters = new List<KendoPageStateFilters>()
                    {
                        new KendoPageStateFilters { Field = "string1", Operator = "contains", Value = "x" },
                        new KendoPageStateFilters { Field = "string1", Operator = "startswith", Value = "y" },
                        new KendoPageStateFilters {
                            Logic = "or",
                            Filters = new List<KendoPageStateFilters>()
                            {
                                new KendoPageStateFilters { Field = "int1", Operator = "eq", Value = "1000 "},
                                new KendoPageStateFilters { Field = "int1", Operator = "gt", Value = "1000 "},
                            }
                        }
                    }
                },
                Sort = new List<KendoPageStateSort>()
                {
                    new KendoPageStateSort() { Field = "string1", Dir = "asc" },
                },
                Skip = 0,
                Take = 1000
            };

            var predicate = state.ToPredicateExpression<SampleEntity>();
            var expression = state.ToExpression<SampleEntity>();
        }

        [Fact]
        public void Expr_KendoPageState_Success_Sort()
        {
            var state = new KendoPageState()
            {
                Sort = new List<KendoPageStateSort>()
                {
                    new KendoPageStateSort() { Field = "guid1", Dir = "asc" },
                    new KendoPageStateSort() { Field = "guid2", Dir = "desc" },
                    new KendoPageStateSort() { Field = "date1", Dir = "asc" },
                    new KendoPageStateSort() { Field = "date2", Dir = "desc" },
                    new KendoPageStateSort() { Field = "int1", Dir = "asc" },
                    new KendoPageStateSort() { Field = "int2", Dir = "desc" },
                    new KendoPageStateSort() { Field = "decimal1", Dir = "asc" },
                    new KendoPageStateSort() { Field = "decimal2", Dir = "desc" },
                    new KendoPageStateSort() { Field = "bool1", Dir = "asc" },
                    new KendoPageStateSort() { Field = "bool2", Dir = "desc" },
                    new KendoPageStateSort() { Field = "string1", Dir = "asc" },
                },
                Skip = 0,
                Take = 1000
            };

            var expression = state.ToExpression<SampleEntity>();
        }

        [Fact]
        public void Expr_KendoPageState_Success_Sort_NoDir()
        {
            var control = new KendoPageState()
            {
                Sort = new List<KendoPageStateSort>()
                {
                    new KendoPageStateSort() { Field = "guid1", Dir = "asc" },
                    new KendoPageStateSort() { Field = "guid2", Dir = "asc" },
                    new KendoPageStateSort() { Field = "date1", Dir = "asc" },
                    new KendoPageStateSort() { Field = "date2", Dir = "asc" },
                    new KendoPageStateSort() { Field = "int1", Dir = "asc" },
                    new KendoPageStateSort() { Field = "int2", Dir = "asc" },
                    new KendoPageStateSort() { Field = "decimal1", Dir = "asc" },
                    new KendoPageStateSort() { Field = "decimal2", Dir = "asc" },
                    new KendoPageStateSort() { Field = "bool1", Dir = "asc" },
                    new KendoPageStateSort() { Field = "bool2", Dir = "asc" },
                    new KendoPageStateSort() { Field = "string1", Dir = "asc" },
                },
                Skip = 0,
                Take = 1000
            };

            var state = new KendoPageState()
            {
                Sort = new List<KendoPageStateSort>()
                {
                    new KendoPageStateSort() { Field = "guid1" },
                    new KendoPageStateSort() { Field = "guid2" },
                    new KendoPageStateSort() { Field = "date1" },
                    new KendoPageStateSort() { Field = "date2" },
                    new KendoPageStateSort() { Field = "int1" },
                    new KendoPageStateSort() { Field = "int2" },
                    new KendoPageStateSort() { Field = "decimal1" },
                    new KendoPageStateSort() { Field = "decimal2" },
                    new KendoPageStateSort() { Field = "bool1" },
                    new KendoPageStateSort() { Field = "bool2" },
                    new KendoPageStateSort() { Field = "string1" },
                },
                Skip = 0,
                Take = 1000
            };

            var validate = control.ToExpression<SampleEntity>();
            var expression = state.ToExpression<SampleEntity>();

            Assert.Equal(validate.Body.ToString(), expression.Body.ToString());
        }
    }
}
